namespace MailPosterAPI.Models;

public class Email
{
    public int Id { get; set; }

    public string To { get; set; } = null!;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public string SentByUserId { get; set; } = null!;
    public string? SentByEmail { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}