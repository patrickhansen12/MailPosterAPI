namespace MailPosterAPI.Services;

public interface IEmailService
{
    Task SendAsync(SendEmailDto dto, string userId, string? userEmail);
}