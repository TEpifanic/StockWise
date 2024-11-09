using AutoMapper;
using StockWise.Common.DTOs;
using StockWise.Common.Mapping;
using StockWise.Domain.Entities;
using Xunit;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfile>());
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfile>());
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_Product_To_ProductDto_IsValid()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test",
            Description = "Description",
            Price = 10.00m,
            ImageUrl = "http://example.com/image.jpg",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = _mapper.Map<ProductDto>(product);

        // Assert
        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Name, dto.Name);
        Assert.Equal(product.Description, dto.Description);
        Assert.Equal(product.Price, dto.Price);
        Assert.Equal(product.ImageUrl, dto.ImageUrl);
    }

    [Fact]
    public void Map_CreateProductDto_To_Product_IsValid()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            Name = "Test",
            Description = "Description",
            Price = 10.00m,
            ImageUrl = "http://example.com/image.jpg"
        };

        // Act
        var product = _mapper.Map<Product>(createDto);

        // Assert
        Assert.Equal(0, product.Id); // Id should be default
        Assert.Equal(createDto.Name, product.Name);
        Assert.Equal(createDto.Description, product.Description);
        Assert.Equal(createDto.Price, product.Price);
        Assert.Equal(createDto.ImageUrl, product.ImageUrl);
        Assert.NotEqual(default, product.CreatedAt);
        Assert.Null(product.UpdatedAt);
    }

    [Fact]
    public void Map_UpdateProductDto_To_Product_IsValid()
    {
        // Arrange
        var updateDto = new UpdateProductDto
        {
            Name = "Updated Test",
            Price = 20.00m
        };

        var existingProduct = new Product
        {
            Id = 1,
            Name = "Original Test",
            Description = "Original Description",
            Price = 10.00m,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        // Act
        _mapper.Map(updateDto, existingProduct);

        // Assert
        Assert.Equal(1, existingProduct.Id);
        Assert.Equal(updateDto.Name, existingProduct.Name);
        Assert.Equal("Original Description", existingProduct.Description); // Should keep original value
        Assert.Equal(updateDto.Price, existingProduct.Price);
        Assert.NotEqual(default, existingProduct.UpdatedAt);
    }
}