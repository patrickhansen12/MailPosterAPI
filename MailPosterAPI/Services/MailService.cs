using MailPosterAPI.Configuration;
using MailPosterAPI.Models;
using Microsoft.Extensions.Options;
using MailPosterAPI.Data;
using MailPosterAPI.DTOs;
using MailPosterAPI.Services.Results;
using MailPosterAPI.Services.Clients;
using Microsoft.EntityFrameworkCore;

namespace MailPosterAPI.Services;

public class MailService : IMailService
{
    private readonly BrevoOptions _options;
    private readonly ApplicationDbContext _context;
    private readonly BrevoClient _brevoClient;

    public MailService(
        IOptions<BrevoOptions> options,
        BrevoClient brevoClient,
        ApplicationDbContext context)
    {
        _brevoClient = brevoClient;
        _options = options.Value;
        _context = context;
    }

    public async Task<MailResult> SendAsync(
        SendMailRequest dto,
        string userId,
        string userEmail)
    {
        // whitelist check - turned off for better demoes
        // if (!string.Equals(dto.To,
        //         _options.AllowedRecipient,
        //         StringComparison.OrdinalIgnoreCase))
        // {
        //     return MailResult.Fail("Demo mode: Only white listed emails can be used");
        // }

        try
        {
            // Sends using a whitelisted system email on brevo
            var response = await _brevoClient.SendEmailAsync(
                fromEmail: _options.SystemEmail,      // ingen-reply@outlook.dk
                fromName: _options.SystemName,       
                toEmail: dto.To,
                toName: "Modtager",
                subject: dto.Subject,
                body: dto.Body);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return MailResult.Fail($"Mail provider error: {error}");
            }
            
            // Saves the email in database
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
        catch (Exception ex)
        {
            return MailResult.Fail($"Mail provider error: {ex.Message}");
        }
    }

    public async Task<List<Mail>> GetByUserAsync(string userEmail)
    {
        return await _context.Emails
            .Where(e => e.SenderEmail == userEmail)  // Finds all the emails sent by the user.
            .OrderByDescending(e => e.SentAt)
            .Select(e => new Mail
            {
                RecipientEmail = e.RecipientEmail,
                Subject = e.Subject,
                Body = e.Body,
                SentAt = e.SentAt
            })
            .ToListAsync();
    }
}