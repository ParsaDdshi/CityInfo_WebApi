using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

public class CityInfoDbContext : DbContext
{
    public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options)
    :base(options) { }
    
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;
    public DbSet<CityInfoUser> CityInfoUsers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City("Tehran")
            {
                CityId = 1,
                Description = "Tehran is capital of Iran"
            },
            new City("Shiraz")
            {
                CityId = 2,
                Description = "Shiraz is one of iran's historical city"
            },
            new City("Rasht")
            {
                CityId = 3,
                Description = "Rasht is famous for its nature"
            }
        );

        modelBuilder.Entity<PointOfInterest>().HasData(
            new PointOfInterest("Milad Tower")
            {
                PointId = 1,
                CityId = 1,
                Description = "Milad Tower is the tallest tower in iran",
            },
            
            new PointOfInterest("Takht-e-jamshid")
            {
                PointId = 2,
                CityId = 2,
                Description = "Takht-e-jamshid is one of iran's historical place"
            },
            new PointOfInterest("Kouroush's Tomb")
            {
                PointId = 3,
                CityId = 2,
                Description = "Koursoush was one of hakhamanesh kings"
            },
            
            new PointOfInterest("Shahrdari-Squre")
            {
                PointId = 4,
                CityId = 3,
                Description = "Shahrdari-Squre is a famous square in Rasht"
            }
        );

        modelBuilder.Entity<CityInfoUser>().HasData(
            new CityInfoUser()
            {
              UserId  = 1,
              UserName = "admin",
              FirstName = "admin",
              LastName = "admin",
              Password = "admin"
            }
        );
        base.OnModelCreating(modelBuilder);
    }
}