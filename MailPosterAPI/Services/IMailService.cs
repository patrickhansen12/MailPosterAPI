using MailPosterAPI.DTOs;

namespace MailPosterAPI.Services;

public interface IMailService
{
    Task SendAsync(SendMailRequest dto, string userId, string userEmail);
}