namespace MailPosterAPI.DTOs;

public class SendEmailDto
{
    public string To { get; set; } = null!;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}