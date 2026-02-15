namespace MailPosterAPI.Models;

public class Email
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
    public string RecipientEmail { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;

    public DateTime SentAt { get; set; }
}