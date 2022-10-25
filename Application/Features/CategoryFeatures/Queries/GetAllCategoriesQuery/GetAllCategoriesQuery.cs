using Application.ViewModels;
using Application.Filter;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQuery : IRequest<PagedResponse<IEnumerable<GetAllCategoriesQueryVM>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }
        public string? Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CreatedBy { get; set; }

        internal class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PagedResponse<IEnumerable<GetAllCategoriesQueryVM>>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllCategoriesQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PagedResponse<IEnumerable<GetAllCategoriesQueryVM>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
            {
                var list = (from c in _context.Categories
                            where (string.IsNullOrEmpty(query.Name) || c.Name.ToLower().Contains(query.Name.ToLower()))
                            && (string.IsNullOrEmpty(query.CreatedBy) || c.CreatedBy.Contains(query.CreatedBy))
                            && (!query.FromDate.HasValue || c.CreatedOn <= query.FromDate.Value)
                            && (!query.ToDate.HasValue || c.CreatedOn >= query.ToDate.Value)
                            select new GetAllCategoriesQueryVM()
                            {
                                Id = c.Id,
                                Name = c.Name,
                                CreatedBy = c.CreatedBy,
                                CreatedOn = c.CreatedOn,
                                Products = (from p in _context.Products
                                            join v in _context.Categories
                                            on p.CategoryId equals c.Id
                                            select new ProductVM
                                            {
                                                Id = p.Id,
                                                ProductName = p.Name,
                                                Barcode = p.Barcode,
                                                CategoryName = c.Name,
                                                CreatedDate = p.CreatedOn,
                                                Description = p.Description,
                                                Quantity = p.Quantity,
                                                Price = p.Price,
                                                Rate = p.Rate
                                            }).ToList()
                            });
                list = query.Order switch
                {
                    "asc" => query.SortBy switch
                    {
                        "Name" => list.OrderBy(x => x.Name),
                        "Date" => list.OrderBy(x => x.CreatedOn)
                    },
                    "desc" => query.SortBy switch
                    {
                        "Name" => list.OrderByDescending(x => x.Name),
                        "Date" => list.OrderByDescending(x => x.CreatedOn)
                    },
                    _ => list
                };
                var total = list.Count();
                var rs = await list.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetAllCategoriesQueryVM>>(list, query.PageNumber, query.PageSize, total));
            }
        }
    }
}