using Application.ViewModels;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.CategoryFeatures.Queries.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdQuery : IRequest<Response<GetProductsByCategoryIdQueryVM>>
    {
        public int Id { get; set; }

        internal class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, Response<GetProductsByCategoryIdQueryVM>>
        {
            private readonly ApplicationDbContext _context;

            public GetProductsByCategoryIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<GetProductsByCategoryIdQueryVM>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
            {
                var list = await (from c in _context.Categories
                                  where c.Id == request.Id
                                  select new GetProductsByCategoryIdQueryVM
                                  {
                                      CategoryId = c.Id,
                                      CategoryName = c.Name,
                                      Products = (from p in _context.Products
                                                  join ct in _context.Categories
                                                  on p.CategoryId equals ct.Id
                                                  where p.CategoryId == request.Id
                                                  select new ProductVM
                                                  {
                                                      ProductName = p.Name,
                                                      CategoryName = c.Name,
                                                      Barcode = p.Barcode,
                                                      Description = p.Description,
                                                      Rate = p.Rate,
                                                      Price = p.Price,
                                                      Quantity = p.Quantity,
                                                      CreatedDate = p.CreatedOn
                                                  }).ToList()
                                  }).FirstOrDefaultAsync();
                return new Response<GetProductsByCategoryIdQueryVM>(list);
            }
        }
    }
}