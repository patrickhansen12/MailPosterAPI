namespace MailPosterAPI.Configuration;

public class MailgunOptions
{
    public string Domain { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public string AllowedRecipient { get; set; } = null!;

}