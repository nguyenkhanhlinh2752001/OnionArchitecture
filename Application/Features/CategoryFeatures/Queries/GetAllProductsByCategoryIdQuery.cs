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

namespace Application.Features.CategoryFeatures.Queries
{
    public class GetAllProductsByCategoryIdQuery: IRequest<IEnumerable<ProductDTO>>
    {
        public int Id { get; set; }
        public class GetAllProductsByCategoryIdQueryHandler : IRequestHandler<GetAllProductsByCategoryIdQuery, IEnumerable<ProductDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllProductsByCategoryIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsByCategoryIdQuery query, CancellationToken cancellationToken)
            {
                var obj = await (from p in _context.Products
                                 join c in _context.Categories on p.CategoryId equals c.Id
                                 where c.Id == query.Id && p.IsDeleted == false && c.IsDeleted == false
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
                return obj.AsReadOnly();
            }
        }
    }
}
