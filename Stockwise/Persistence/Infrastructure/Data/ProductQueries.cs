using Microsoft.EntityFrameworkCore;
using StockWise.Domain.Entities;

namespace StockWise.Persistence.Infrastructure.Data
{
    public class ProductQueries
    {
        private readonly ApplicationDbContext _context;

        public ProductQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _context.Products.ToListAsync();

            return await _context.Products
                .Where(p => p.Name.Contains(searchTerm) ||
                           (p.Description != null && p.Description.Contains(searchTerm)))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .OrderBy(p => p.Price)
                .ToListAsync();
        }
    }
}
