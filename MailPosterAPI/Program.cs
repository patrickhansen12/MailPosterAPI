using MailPosterAPI.Services;
using MailPosterAPI.Data;
using Microsoft.EntityFrameworkCore;
using MailPosterAPI.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.Configure<MailgunOptions>(
    builder.Configuration.GetSection("Mailgun")
);

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

app.Run();