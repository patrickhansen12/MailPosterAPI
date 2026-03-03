namespace MailPosterAPI.Configuration;

public class BrevoOptions
{
    public const string SectionName = "Brevo";
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.brevo.com/v3/";
    public string SystemEmail { get; set; } = string.Empty;
    public string SystemName { get; set; } = "Mail Poster Demo";
    public string AllowedRecipient { get; set; } = string.Empty;
}