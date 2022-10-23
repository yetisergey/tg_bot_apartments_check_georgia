using Microsoft.EntityFrameworkCore;
using Storage.Flats.Models;

namespace Storage.Flats;

public class FlatContext : DbContext
{
    private readonly string _connectionString;

    public FlatContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbSet<Flat> Flats => Set<Flat>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flat>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();
    }
}
