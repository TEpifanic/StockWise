using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockWise.Common.DTOs;
using StockWise.Persistence.Infrastructure.Data;

namespace StockWise.Commands.Products
{
    public record GetProductsByPriceRangeQuery(decimal MinPrice, decimal MaxPrice) : IRequest<IEnumerable<ProductDto>>;

    public class GetProductsByPriceRangeQueryHandler
        : IRequestHandler<GetProductsByPriceRangeQuery, IEnumerable<ProductDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsByPriceRangeQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(
            GetProductsByPriceRangeQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .Where(p => p.Price >= request.MinPrice && p.Price <= request.MaxPrice)
                .OrderBy(p => p.Price)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}