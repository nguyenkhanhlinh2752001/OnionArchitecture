using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetProductByIdQuery: IRequest<ProductDTO>
    {
        public int Id { get; set; }
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
        {
            private readonly IApplicationDbContext _context;
            public GetProductByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ProductDTO> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
            {
                var obj =  (from p in _context.Products
                                  join c in _context.Categories on p.CategoryId equals c.Id
                                  where p.Id==query.Id && p.IsDeleted == false
                                  select new ProductDTO()
                                  {
                                      Id = p.Id,
                                      ProductName = p.Name,
                                      CategoryName = c.Name,
                                      Barcode = p.Barcode,
                                      Description = p.Description,
                                      Price = p.Price,
                                      Quantity = p.Quantity,
                                      CreatedDate = p.CreatedDate,
                                  }).FirstOrDefault();
                return obj;
            }
        }
    }
}
