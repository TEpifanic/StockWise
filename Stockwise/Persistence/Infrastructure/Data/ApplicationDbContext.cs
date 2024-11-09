using Microsoft.EntityFrameworkCore;
using StockWise.Domain.Entities;

namespace StockWise.Persistence.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                // Remarque : SQLite ne supporte pas nativement decimal
                // EF Core s'occupe de la conversion automatiquement
                entity.Property(e => e.Price).HasConversion<double>();

                entity.HasIndex(e => e.Name);
            });
        }
    }
}
