using CitiesManager.Core.Entities;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DataBaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options){ }

        public ApplicationDbContext() { }

        public virtual DbSet<City> Cities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(new City() { CityId = Guid.Parse("B94B2C7F-B6E3-4CA1-A804-E961F89B22A2"), CityName = "Porto" });
            modelBuilder.Entity<City>().HasData(new City() { CityId = Guid.Parse("06EB4EAC-E753-4D41-ADB3-E33DE57ABC96"), CityName = "Lisbon" });
        }
    }
}
