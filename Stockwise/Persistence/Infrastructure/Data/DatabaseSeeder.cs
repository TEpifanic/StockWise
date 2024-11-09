using Microsoft.EntityFrameworkCore;
using StockWise.Domain.Entities;

namespace StockWise.Persistence.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // S'assurer que la base de données est créée
            await context.Database.EnsureCreatedAsync();

            // Vérifier si des données existent déjà
            if (!await context.Products.AnyAsync())
            {
                // Ajouter des données de test
                var products = new List<Product>
            {
                new Product
                {
                    Name = "iPhone 15",
                    Description = "Dernier iPhone avec des fonctionnalités innovantes",
                    Price = 999.99m,
                    ImageUrl = "https://example.com/iphone15.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Samsung Galaxy S24",
                    Description = "Smartphone Android haut de gamme",
                    Price = 899.99m,
                    ImageUrl = "https://example.com/s24.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "MacBook Pro",
                    Description = "Ordinateur portable professionnel",
                    Price = 1299.99m,
                    ImageUrl = "https://example.com/macbook.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
