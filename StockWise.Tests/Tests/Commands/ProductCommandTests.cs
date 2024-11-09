using Xunit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockWise.Commands.Products;
using StockWise.Domain.Entities;
using StockWise.Persistence.Infrastructure.Data;
using StockWise.Common.Mapping;

namespace StockWise.Tests.Commands
{
    public class ProductCommandTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IMapper _mapper;

        public ProductCommandTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid())
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private async Task CleanDatabase()
        {
            using var context = new ApplicationDbContext(_options);
            context.Products.RemoveRange(context.Products);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateProductCommand_ValidProduct_ReturnsId()
        {
            await CleanDatabase();

            // Arrange
            using var context = new ApplicationDbContext(_options);

            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                ImageUrl = "http://test.com/image.jpg"
            };

            var handler = new CreateProductCommandHandler(context, _mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result > 0);
            var savedProduct = await context.Products.FirstOrDefaultAsync();
            Assert.NotNull(savedProduct);
            Assert.Equal(command.Name, savedProduct.Name);
            Assert.Equal(command.Description, savedProduct.Description);
            Assert.Equal(command.Price, savedProduct.Price);
            Assert.Equal(command.ImageUrl, savedProduct.ImageUrl);
        }

        [Fact]
        public async Task UpdateProductCommand_ExistingProduct_UpdatesSuccessfully()
        {
            await CleanDatabase();

            // Arrange
            using var context = new ApplicationDbContext(_options);

            var existingProduct = new Product
            {
                Name = "Original Product",
                Description = "Original Description",
                Price = 99.99m
            };
            context.Products.Add(existingProduct);
            await context.SaveChangesAsync();

            var command = new UpdateProductCommand
            {
                Id = existingProduct.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 149.99m,
                ImageUrl = "http://test.com/updated.jpg"
            };

            var handler = new UpdateProductCommandHandler(context, _mapper);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await context.Products.FindAsync(existingProduct.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal(command.Name, updatedProduct.Name);
            Assert.Equal(command.Description, updatedProduct.Description);
            Assert.Equal(command.Price, updatedProduct.Price);
            Assert.Equal(command.ImageUrl, updatedProduct.ImageUrl);
        }

        [Fact]
        public async Task UpdateProductCommand_NonExistentProduct_ThrowsKeyNotFoundException()
        {
            await CleanDatabase();

            // Arrange
            using var context = new ApplicationDbContext(_options);

            var command = new UpdateProductCommand
            {
                Id = 999,
                Name = "Updated Product"
            };

            var handler = new UpdateProductCommandHandler(context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteProductCommand_ExistingProduct_DeletesSuccessfully()
        {
            await CleanDatabase();

            // Arrange
            using var context = new ApplicationDbContext(_options);

            var product = new Product
            {
                Name = "Test Product",
                Price = 99.99m
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var handler = new DeleteProductCommandHandler(context);

            // Act
            await handler.Handle(new DeleteProductCommand(product.Id), CancellationToken.None);

            // Assert
            var deletedProduct = await context.Products.FindAsync(product.Id);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task DeleteProductCommand_NonExistentProduct_ThrowsKeyNotFoundException()
        {
            await CleanDatabase();

            // Arrange
            using var context = new ApplicationDbContext(_options);
            var handler = new DeleteProductCommandHandler(context);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(new DeleteProductCommand(999), CancellationToken.None));
        }

        [Fact]
        public async Task GetProductsQuery_ReturnsAllProducts()
        {
            await CleanDatabase();

            // Arrange
            using var context = new ApplicationDbContext(_options);

            var products = new List<Product>
            {
                new Product { Name = "Product 1", Price = 99.99m },
                new Product { Name = "Product 2", Price = 149.99m }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var handler = new GetProductsQueryHandler(context, _mapper);

            // Act
            var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Collection(result,
                item => Assert.Equal("Product 1", item.Name),
                item => Assert.Equal("Product 2", item.Name)
            );
        }

        [Fact]
        public async Task GetProductsByPriceRange_ReturnsFilteredProducts()
        {
            await CleanDatabase();

            // Arrange
            using var context = new ApplicationDbContext(_options);

            var products = new List<Product>
            {
                new Product { Name = "Product 1", Price = 50m },
                new Product { Name = "Product 2", Price = 150m },
                new Product { Name = "Product 3", Price = 100m }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var handler = new GetProductsByPriceRangeQueryHandler(context, _mapper);

            // Act
            var result = await handler.Handle(new GetProductsByPriceRangeQuery(0, 100), CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.DoesNotContain(result, p => p.Price > 100m);
            Assert.Collection(result,
                item => Assert.Equal("Product 1", item.Name),
                item => Assert.Equal("Product 3", item.Name)
            );
        }
    }
}