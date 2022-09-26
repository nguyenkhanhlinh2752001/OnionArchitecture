using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetAllProductsQuery: IRequest<IEnumerable<ProductDTO>>
    {

        public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllProductsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
            {
                var list = await (from p in _context.Products
                                  join c in _context.Categories on p.CategoryId equals c.Id
                                  where p.IsDeleted==false
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
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}
