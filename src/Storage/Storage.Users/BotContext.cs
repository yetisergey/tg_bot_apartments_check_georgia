using Microsoft.EntityFrameworkCore;
using Storage.Bot.Models;

namespace Storage.Bot;

public class BotContext : DbContext
{
    public BotContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Filter> Filters => Set<Filter>();

    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne<Filter>()
            .WithOne(x => x.User);

        modelBuilder.Entity<City>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();
        modelBuilder.Entity<User>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();
        modelBuilder.Entity<Filter>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        modelBuilder.Entity<City>().HasData(
            new City
            {
                Id = 1,
                IsDefault = true,
                Name = "Tbilisi",
                NameGe = "თბილისი",
                Cities = "1996871",
                GID = "1996871",
                Regions = "687578743",
                FullRegions = "687578743",
                Districts = "62176122.319380261.58416723.2953929439.58420997.152297954.61645269.6273968347.58416582.58416672.58377946",
                MapC = "41.73188365,44.8368762993663",
            },
            new City
            {
                Id = 2,
                IsDefault = false,
                Name = "Batumi",
                NameGe = "ბათუმი",
                Districts = "776481390.776472116.776471185.777654897.776734274.776998491.776460995.776458944.776463102.776465448",
                GID = "8742159",
                Cities = "8742159",
                FullRegions = string.Empty,
                Regions = string.Empty,
                MapC = "41.73188365,44.8368762993663",
            });
    }
}
