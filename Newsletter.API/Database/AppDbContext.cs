using Microsoft.EntityFrameworkCore;
using Newsletter.API.Sagas;

namespace Newsletter.API.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<NewsletterOnboardingSagaData>().HasKey(x => x.CorrelationId);
	}

	public DbSet<Subscriber> Subscribers { get; set; }

    public DbSet<NewsletterOnboardingSagaData> SagaData { get; set; }
}
