using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.ProductFeatures.Queries.GetProductsSoldQuery
{
    public class GetProductsSoldQuery : IRequest<IEnumerable<GetProductsSoldViewModel>>
    {
        internal class GetProductsSoldQueryHandler : IRequestHandler<GetProductsSoldQuery, IEnumerable<GetProductsSoldViewModel>>
        {
            private readonly ApplicationDbContext _context;

            public GetProductsSoldQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetProductsSoldViewModel>> Handle(GetProductsSoldQuery query, CancellationToken token)
            {
                var list = await (from p1 in _context.Products
                                  join c in _context.Categories
                                  on p1.CategoryId equals c.Id
                                  select new GetProductsSoldViewModel
                                  {
                                      Id = p1.Id,
                                      Name = p1.Name,
                                      Price = p1.Price,
                                      Category = c.Name,
                                      TotalQuantity = (from p2 in _context.Products
                                                       join od in _context.OrderDetails
                                                       on p2.Id equals od.ProductId
                                                       where p2.Id == od.ProductId && p2.Id == p1.Id
                                                       select od.Quantity).Sum()
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}