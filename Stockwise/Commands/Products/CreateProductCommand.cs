using AutoMapper;
using MediatR;
using StockWise.Domain.Entities;
using StockWise.Persistence.Infrastructure.Data;

namespace StockWise.Commands.Products
{
    public record CreateProductCommand : IRequest<int>
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? ImageUrl { get; init; }
        public decimal Price { get; init; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }

}
