using System.Text;
using System.Text.Json;

namespace MailPosterAPI.Services.Clients;

public class BrevoClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public BrevoClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
        
        var apiKey = _config["Brevo:ApiKey"];
        _httpClient.BaseAddress = new Uri(_config["Brevo:BaseUrl"] ?? "https://api.brevo.com/v3/");
        _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);
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