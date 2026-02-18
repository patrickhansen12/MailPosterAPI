using MailPosterAPI.Configuration;
using MailPosterAPI.Models;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailPosterAPI.Data;
using MailPosterAPI.DTOs;
using MimeKit;


namespace MailPosterAPI.Services;

public class MailService : IMailService
{
    private readonly MailgunOptions _options;
    private readonly ApplicationDbContext _context;


    public MailService(
        IOptions<MailgunOptions> options,
        ApplicationDbContext context)
    {
        _options = options.Value;
        _context = context;
    }

    public async Task SendAsync(
        SendMailRequest dto,
        string userId,
        string userEmail)
    {
        //exit first, if the email is not azllowed I throw an exception
        if (dto.To != _options.AllowedRecipient)
        {
            throw new InvalidOperationException("Recipient not allowed.");
        }
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(userEmail, userEmail));
        message.To.Add(MailboxAddress.Parse(dto.To));
        message.Subject = dto.Subject;

        message.Body = new TextPart("plain")
        {
            Text = dto.Body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _options.Host,
            _options.Port,
            SecureSocketOptions.StartTls);

        Console.WriteLine($"HOST: {_options.Host}");
        Console.WriteLine($"USER: {_options.Username}");
        Console.WriteLine($"PASS: {_options.Password?.Length}");
        
        await client.AuthenticateAsync(
            _options.Username,
            _options.Password);

        
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        // 2️⃣ Gem i database
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
    }
}