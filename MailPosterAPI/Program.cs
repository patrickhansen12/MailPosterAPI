using MailPosterAPI.Services;
using MailPosterAPI.Data;
using Microsoft.EntityFrameworkCore;
using MailPosterAPI.Configuration;
using MailPosterAPI.Services.Clients;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Database - PostgreSQL til Render
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

// Converter Render's PostgreSQL URL til EF Core format
if (connectionString != null && (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://")))
{
    try 
    {
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':');
        
        connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};" +
                           $"Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR parsing PostgreSQL URL: {ex.Message}");
    }
}


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Brevo configuration
builder.Services.Configure<BrevoOptions>(options =>
{
    options.ApiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY") 
                     ?? builder.Configuration["Brevo:ApiKey"];
    options.SystemEmail = Environment.GetEnvironmentVariable("BREVO_SYSTEM_EMAIL")
                          ?? builder.Configuration["Brevo:SystemEmail"];
    options.SystemName = Environment.GetEnvironmentVariable("BREVO_SYSTEM_NAME")
                         ?? builder.Configuration["Brevo:SystemName"];
    //As I dont need a AllowedRecipient right now this is removed:
    // options.AllowedRecipient = Environment.GetEnvironmentVariable("BREVO_ALLOWED_RECIPIENT")
    //                            ?? builder.Configuration["Brevo:AllowedRecipient"];
});

builder.Services.AddHttpClient<BrevoClient>();

// Services
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IDraftService, DraftService>(); // Fjernet duplicate

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Sørg for at databasen er oprettet (kun til demo - ikke til produktion!)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated(); // Creates the database if none is there
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();