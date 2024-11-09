using StockWise.Common.DTOs;
using StockWise.Common.Validation;
using Xunit;

public class ProductValidatorTests
{
    private readonly CreateProductDtoValidator _createValidator;
    private readonly UpdateProductDtoValidator _updateValidator;

    public ProductValidatorTests()
    {
        _createValidator = new CreateProductDtoValidator();
        _updateValidator = new UpdateProductDtoValidator();
    }

    [Fact]
    public async Task CreateValidator_ValidProduct_ShouldNotHaveErrors()
    {
        // Arrange
        var product = new CreateProductDto
        {
            Name = "Test Product",
            Description = "Description",
            Price = 10.00m,
            ImageUrl = "http://example.com/image.jpg"
        };

        // Act
        var result = await _createValidator.ValidateAsync(product);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "Name ne doit pas être vide")]
    [InlineData("Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua", "Le nom du produit est requis et ne doit pas dépasser 100 caractères.")]
    public async Task CreateValidator_InvalidName_ShouldHaveErrors(string name, string expectedError)
    {
        // Arrange
        var product = new CreateProductDto { Name = name, Price = 10.00m };

        // Act
        var result = await _createValidator.ValidateAsync(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == expectedError);
    }

    [Theory]
    [InlineData(-1, "Le prix doit être supérieur à 0.")]
    [InlineData(0, "Le prix doit être supérieur à 0.")]
    public async Task CreateValidator_InvalidPrice_ShouldHaveErrors(decimal price, string expectedError)
    {
        // Arrange
        var product = new CreateProductDto
        {
            Name = "Test Product",
            Price = price
        };

        // Act
        var result = await _createValidator.ValidateAsync(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == expectedError);
    }

    [Theory]
    [InlineData("not a url", "L'URL de l'image n'est pas valide.")]
    [InlineData("invalid.url", "L'URL de l'image n'est pas valide.")]
    public async Task CreateValidator_InvalidImageUrl_ShouldHaveErrors(string imageUrl, string expectedError)
    {
        // Arrange
        var product = new CreateProductDto
        {
            Name = "Test Product",
            Price = 10.00m,
            ImageUrl = imageUrl
        };

        // Act
        var result = await _createValidator.ValidateAsync(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == expectedError);
    }
}