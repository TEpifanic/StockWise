using MediatR;
using StockWise.Persistence.Infrastructure.Data;

namespace StockWise.Commands.Products
{
    public record DeleteProductCommand(int Id) : IRequest;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
