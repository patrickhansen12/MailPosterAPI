using MailPosterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MailPosterAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Email> Emails => Set<Email>();
}