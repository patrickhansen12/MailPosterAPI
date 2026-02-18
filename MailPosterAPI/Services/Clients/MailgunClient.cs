using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using MailPosterAPI.Configuration;

namespace MailPosterAPI.Services.Clients;

public class MailgunClient
{
    private readonly HttpClient _httpClient;
    private readonly MailgunOptions _options;

    public MailgunClient(
        HttpClient httpClient,
        IOptions<MailgunOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        var byteArray = System.Text.Encoding.ASCII.GetBytes($"api:{_options.ApiKey}");
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(byteArray));
    }

    public async Task<HttpResponseMessage> SendEmailAsync(
        string from,
        string to,
        string subject,
        string body)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", from),
            new KeyValuePair<string, string>("to", to),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("text", body)
        });

        return await _httpClient.PostAsync(
            $"https://api.mailgun.net/v3/{_options.Domain}/messages",
            content);
    }
}