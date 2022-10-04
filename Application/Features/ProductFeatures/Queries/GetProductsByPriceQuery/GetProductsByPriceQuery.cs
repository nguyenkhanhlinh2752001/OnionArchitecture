using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.ProductFeatures.Queries.GetProductsByPriceQuery
{
    public class GetProductsByPriceQuery : IRequest<IEnumerable<GetProductsByPriceViewModel>>
    {
        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; }

        public class GetProductsByPriceQueryHandler : IRequestHandler<GetProductsByPriceQuery, IEnumerable<GetProductsByPriceViewModel>>
        {
            private readonly ApplicationDbContext _context;

            public GetProductsByPriceQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetProductsByPriceViewModel>> Handle(GetProductsByPriceQuery query, CancellationToken token)
            {
                var list = await (from p in _context.Products
                                  join c in _context.Categories
                                  on p.CategoryId equals c.Id
                                  where p.Price >= query.FromPrice && p.Price <= query.ToPrice
                                  select new GetProductsByPriceViewModel
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Price = p.Price,
                                      Category = c.Name
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}