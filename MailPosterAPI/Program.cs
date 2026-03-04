using MailPosterAPI.Services;
using MailPosterAPI.Data;
using Microsoft.EntityFrameworkCore;
using MailPosterAPI.Configuration;
using MailPosterAPI.Services.Clients;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

    
builder.Services.Configure<BrevoOptions>(
    builder.Configuration.GetSection(BrevoOptions.SectionName));

builder.Services.AddHttpClient<BrevoClient>();

// Services
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IDraftService, DraftService>();

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

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Health check endpoint (Used to ensure render awakes faster)
app.MapGet("/health", () => Results.Ok(new { 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    environment = builder.Environment.EnvironmentName
}));

app.Run();