using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.ProductFeatures.Queries.GetProductsSearchQuery
{
    public class GetProductsSearchQuery : IRequest<IEnumerable<GetProductsSearchQueryVM>>
    {
        public string? ProductName { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public string? CategoryName { get; set; }

        public class GetProductsSearchQueryHandler : IRequestHandler<GetProductsSearchQuery, IEnumerable<GetProductsSearchQueryVM>>
        {
            private readonly ApplicationDbContext _context;

            public GetProductsSearchQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetProductsSearchQueryVM>> Handle(GetProductsSearchQuery query, CancellationToken cancellationToken)
            {
                var list = await (from p in _context.Products
                                  join c in _context.Categories
                                    on p.CategoryId equals c.Id
                                  where (string.IsNullOrEmpty(query.ProductName) || p.Name.ToLower().Contains(query.ProductName.ToLower()))
                                  && (!query.FromPrice.HasValue || p.Price >= query.FromPrice.Value)
                                  && (!query.ToPrice.HasValue || p.Price <= query.ToPrice.Value)
                                  && (string.IsNullOrEmpty(query.CategoryName) || c.Name.ToLower().Contains(query.CategoryName.ToLower()))
                                  select new GetProductsSearchQueryVM
                                  {
                                      ProductName = p.Name,
                                      CategoryName = c.Name,
                                      Price = p.Price,
                                      Rate = p.Rate
                                  }).ToListAsync();
                return list;
            }
        }
    }
}