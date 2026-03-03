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

app.Run();