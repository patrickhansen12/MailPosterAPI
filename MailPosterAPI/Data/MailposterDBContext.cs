namespace MailPosterAPI.Data;

public class MailposterDbContext : DbContext
{
    public MailposterDbContext(DbContextOptions<MailposterDbContext> options)
        : base(options)
    {
    }

    public DbSet<Email> Emails => Set<Email>();
}