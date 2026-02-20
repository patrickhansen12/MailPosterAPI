using MailPosterAPI.DTOs;
using MailPosterAPI.Services.Results;

namespace MailPosterAPI.Services;

public interface IMailService
{
    Task<MailResult> SendAsync(SendMailRequest dto, string userId, string userEmail);
    Task<List<Mail>> GetByUserAsync(string userEmail);
}