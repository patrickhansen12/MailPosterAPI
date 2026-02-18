using MailPosterAPI.Configuration;
using MailPosterAPI.Models;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailPosterAPI.Data;
using MailPosterAPI.DTOs;
using MimeKit;
using MailPosterAPI.Services.Results;
using MailPosterAPI.Services.Clients;


namespace MailPosterAPI.Services;

public class MailService : IMailService
{
    private readonly MailgunOptions _options;
    private readonly ApplicationDbContext _context;
    private readonly MailgunClient _mailgunClient;


    public MailService(
        IOptions<MailgunOptions> options,
        MailgunClient mailgunClient,
        ApplicationDbContext context)
    {
        _mailgunClient = mailgunClient;
        _options = options.Value;
        _context = context;
    }

    public async Task<MailResult> SendAsync(
        SendMailRequest dto,
        string userId,
        string userEmail)
    {
        // Whitelist check
        if (!string.Equals(dto.To,
                _options.AllowedRecipient,
                StringComparison.OrdinalIgnoreCase))
        {
            return MailResult.Fail("Recipient not allowed.");
        }

        try
        {
            var response = await _mailgunClient.SendEmailAsync(
                from: userEmail,
                to: dto.To,
                subject: dto.Subject,
                body: dto.Body);

            if (!response.IsSuccessStatusCode)
            {
                return MailResult.Fail("Mail provider error.");
            }

            // Gem i database
            var email = new Email
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SenderEmail = userEmail,
                RecipientEmail = dto.To,
                Subject = dto.Subject,
                Body = dto.Body,
                SentAt = DateTime.UtcNow
            };

            _context.Emails.Add(email);
            await _context.SaveChangesAsync();

            return MailResult.Ok();
        }
        catch
        {
            return MailResult.Fail("Mail provider error.");
        }
    }
}