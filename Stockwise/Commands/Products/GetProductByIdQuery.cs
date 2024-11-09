using AutoMapper;
using MediatR;
using StockWise.Common.DTOs;
using StockWise.Persistence.Infrastructure.Data;

namespace StockWise.Commands.Products
{
    public record GetProductByIdQuery(int Id) : IRequest<ProductDto>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
            if (product == null)
                return null;

            return _mapper.Map<ProductDto>(product);
        }
    }
}