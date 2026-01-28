using MailPosterAPI.Data;

namespace MailPosterAPI.Services;

public class EmailService : IEmailService
{
    private readonly MailposterDbContext _db;

    public EmailService(MailposterDbContext db)
    {
        _db = db;
    }

    public async Task SendAsync(SendEmailDto dto, string userId, string? userEmail)
    {
        // TODO: SMTP / Mailtrap integration senere

        var email = new Email
        {
            To = dto.To,
            Subject = dto.Subject,
            Message = dto.Message,
            SentByUserId = userId,
            SentByEmail = userEmail
        };

        _db.Emails.Add(email);
        await _db.SaveChangesAsync();
    }
}