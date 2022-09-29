﻿using Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.CategoryFeatures.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryDTO>
    {
        public int Id { get; set; }

        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDTO>
        {
            private readonly ApplicationDbContext _context;

            public GetCategoryByIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<CategoryDTO> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
            {
                var obj = await (from c in _context.Categories
                                 where c.Id == query.Id
                                 select new CategoryDTO
                                 {
                                     CategoryName = c.Name,
                                     Products = (from p in _context.Products
                                                 join ct in _context.Categories
                                                 on p.CategoryId equals ct.Id
                                                 where c.Id == query.Id
                                                 select new ProductDTO
                                                 {
                                                     ProductName = p.Name,
                                                     Barcode = p.Barcode,
                                                     Description = p.Description,
                                                     Rate = p.Rate,
                                                     Price = p.Price,
                                                     Quantity = p.Quantity
                                                 }).ToList()
                                 }).FirstOrDefaultAsync();
                return obj;
            }
        }
    }
}