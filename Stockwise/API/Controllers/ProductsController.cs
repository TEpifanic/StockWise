using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockWise.Commands.Products;
using StockWise.Common.DTOs;
using FluentValidation;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductsController(
        IMediator mediator,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator)
    {
        _mediator = mediator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        if (result == null)
            return NotFound($"Produit avec l'ID {id} non trouvé.");

        return Ok(result);
    }

    [HttpGet("price-range")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetByPriceRange(
    [FromQuery] decimal minPrice = 0,
    [FromQuery] decimal maxPrice = decimal.MaxValue,
    CancellationToken cancellationToken = default)
    {
        var query = new GetProductsByPriceRangeQuery(minPrice, maxPrice);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        // Validation
        var validationResult = await _createValidator.ValidateAsync(createProductDto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new CreateProductCommand
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            ImageUrl = createProductDto.ImageUrl,
            Price = createProductDto.Price
        };

        var productId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = productId }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        // Validation
        var validationResult = await _updateValidator.ValidateAsync(updateProductDto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new UpdateProductCommand
        {
            Id = id,
            Name = updateProductDto.Name,
            Description = updateProductDto.Description,
            ImageUrl = updateProductDto.ImageUrl,
            Price = updateProductDto.Price
        };

        try
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Produit avec l'ID {id} non trouvé.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Produit avec l'ID {id} non trouvé.");
        }
    }
}