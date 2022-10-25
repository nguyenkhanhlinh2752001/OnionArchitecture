using Application.Wrappers;
using MediatR;
using Persistence.Context;

namespace Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Response<GetProductByIdViewModel>>
    {
        public int Id { get; set; }

        internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdViewModel>>
        {
            private readonly ApplicationDbContext _context;

            public GetProductByIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<GetProductByIdViewModel>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
            {
                var result = (from p in _context.Products
                              join c in _context.Categories on p.CategoryId equals c.Id
                              where p.Id == query.Id && p.IsDeleted == false
                              select new GetProductByIdViewModel()
                              {
                                  Id = p.Id,
                                  ProductName = p.Name,
                                  CategoryName = c.Name,
                                  Barcode = p.Barcode,
                                  Description = p.Description,
                                  Price = p.Price,
                                  Quantity = p.Quantity,
                              }).FirstOrDefault();
                return new Response<GetProductByIdViewModel>(result);
            }
        }
    }
}