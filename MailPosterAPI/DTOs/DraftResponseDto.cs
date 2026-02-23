namespace MailPosterAPI.DTOs;

public class DraftResponseDto
{
    public Guid Id { get; set; }

    public string RecipientEmail { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}