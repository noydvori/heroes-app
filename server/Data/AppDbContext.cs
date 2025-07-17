using Microsoft.EntityFrameworkCore;

namespace HeroesApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Hero> Heroes => Set<Hero>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Hero>()
            .Property(h => h.StartingPower)
            .HasPrecision(18, 2); 

        modelBuilder.Entity<Hero>()
            .Property(h => h.CurrentPower)
            .HasPrecision(18, 2); 
    }
}
