using CarsCatalog2.Models;
using Microsoft.EntityFrameworkCore;

namespace CarsCatalog2.Data
{
    public class CarsDbContext : DbContext
    {
        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().HasMany(b => b.Cars).WithOne(c => c.Brand).HasForeignKey(c => c.BrandId);
        }

    }
   
}
 