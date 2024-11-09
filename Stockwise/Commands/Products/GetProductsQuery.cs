using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockWise.Common.DTOs;
using StockWise.Persistence.Infrastructure.Data;

namespace StockWise.Commands.Products
{
    public record GetProductsQuery : IRequest<IEnumerable<ProductDto>>;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
