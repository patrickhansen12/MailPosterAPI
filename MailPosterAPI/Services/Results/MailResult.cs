namespace MailPosterAPI.Services.Results;

public class MailResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }

   //Using these to return a response instead of throwing exceptions
    public static MailResult Ok() =>
        new() { Success = true };

    public static MailResult Fail(string message) =>
        new() { Success = false, ErrorMessage = message };
}