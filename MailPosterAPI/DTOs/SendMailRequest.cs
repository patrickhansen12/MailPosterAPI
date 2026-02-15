namespace MailPosterAPI.DTOs;

public class SendMailRequest
{
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}