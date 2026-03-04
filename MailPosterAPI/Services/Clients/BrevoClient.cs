using System.Text;
using System.Text.Json;
using MailPosterAPI.Configuration;
using Microsoft.Extensions.Options;

namespace MailPosterAPI.Services.Clients;

public class BrevoClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public BrevoClient(HttpClient httpClient, IOptions<BrevoOptions> options)
    {
        _httpClient = httpClient;
        var brevoOptions = options.Value;
    
        _httpClient.BaseAddress = new Uri(brevoOptions.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("api-key", brevoOptions.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
    }
    public async Task<HttpResponseMessage> SendEmailAsync(
        string fromEmail,
        string fromName,
        string toEmail,
        string toName,
        string subject,
        string body)
    {
        var payload = new
        {
            sender = new { 
                email = fromEmail, 
                name = fromName 
            },
            to = new[] { 
                new { 
                    email = toEmail, 
                    name = toName 
                } 
            },
            subject = subject,
            htmlContent = body
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }),
            Encoding.UTF8,
            "application/json");

        return await _httpClient.PostAsync("smtp/email", content);
    }
}