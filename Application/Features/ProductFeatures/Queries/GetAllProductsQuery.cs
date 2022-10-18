using Application.DTOs;
using Application.Filter;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetAllProductsQuery : IRequest<PagedResponse<IEnumerable<ProductDTO>>>
    {
        public PaginationFilter Filter { get; set; }
        public string? ProductName { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public string? CategoryName { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }

        public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<IEnumerable<ProductDTO>>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllProductsQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PagedResponse<IEnumerable<ProductDTO>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
            {
                var validFilter = new PaginationFilter(query.Filter.PageNumber, query.Filter.PageSize);
                var list = (from p in _context.Products
                            join c in _context.Categories on p.CategoryId equals c.Id
                            where (string.IsNullOrEmpty(query.ProductName) || p.Name.ToLower().Contains(query.ProductName.ToLower()))
                            && (!query.FromPrice.HasValue || p.Price >= query.FromPrice.Value)
                            && (!query.ToPrice.HasValue || p.Price <= query.ToPrice.Value)
                            && (string.IsNullOrEmpty(query.CategoryName) || c.Name.ToLower().Contains(query.CategoryName.ToLower()))
                            select new ProductDTO()
                            {
                                Id = p.Id,
                                ProductName = p.Name,
                                CategoryName = c.Name,
                                Barcode = p.Barcode,
                                Description = p.Description,
                                Price = p.Price,
                                Quantity = p.Quantity,
                                CreatedDate = p.CreatedOn,
                            });
                list = query.Order switch
                {
                    "asc" => query.SortBy switch
                    {
                        "Name" => list.OrderBy(x => x.ProductName),
                        "Price" => list.OrderBy(x => x.Price),
                        "Rate" => list.OrderBy(x => x.Rate)
                    },
                    "desc" => query.SortBy switch
                    {
                        "Name" => list.OrderByDescending(x => x.ProductName),
                        "Price" => list.OrderByDescending(x => x.Price),
                        "Rate" => list.OrderByDescending(x => x.Rate)
                    },
                    _ => list
                };
                var total = list.Count();
                var rs = await list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<ProductDTO>>(list, validFilter.PageNumber, validFilter.PageSize, total));
            }
        }
    }
}