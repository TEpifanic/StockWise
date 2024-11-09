using AutoMapper;
using MediatR;
using StockWise.Persistence.Infrastructure.Data;

namespace StockWise.Commands.Products
{
    public record UpdateProductCommand : IRequest
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public string? ImageUrl { get; init; }
        public decimal Price { get; init; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

            _mapper.Map(request, product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
