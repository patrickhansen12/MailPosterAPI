namespace MailPosterAPI.DTOs;

public class UpdateDraftRequestDto
{
    public string RecipientEmail { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}