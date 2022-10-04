using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.ProductFeatures.Queries.GetProductsByNameQuery
{
    public class GetProductsByNameQuery : IRequest<IEnumerable<GetProductsByNameViewModel>>
    {
        public string Name { get; set; }

        public class GetProductsByNameQueryHandler : IRequestHandler<GetProductsByNameQuery, IEnumerable<GetProductsByNameViewModel>>
        {
            private ApplicationDbContext _context;

            public GetProductsByNameQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetProductsByNameViewModel>> Handle(GetProductsByNameQuery query, CancellationToken token)
            {
                var list = await (from p in _context.Products
                                  join c in _context.Categories
                                  on p.CategoryId equals c.Id
                                  where p.Name.Contains(query.Name)
                                  select new GetProductsByNameViewModel
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