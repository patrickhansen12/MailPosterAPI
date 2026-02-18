namespace MailPosterAPI.Configuration;

public class MailgunOptions
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string AllowedRecipient { get; set; } = null!;

}