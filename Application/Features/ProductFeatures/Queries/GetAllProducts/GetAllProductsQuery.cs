using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<PagedResponse<IEnumerable<GetAllProductsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? ProductName { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public decimal? FromRate { get; set; }
        public decimal? ToRate { get; set; }
        public string? CategoryName { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }

        internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<IEnumerable<GetAllProductsViewModel>>>
        {
            private readonly IProductRepository _productRepsitory;
            private readonly ICategoryRepository _categoryRepository;

            public GetAllProductsQueryHandler(IProductRepository productRepsitory, ICategoryRepository categoryRepository)
            {
                _productRepsitory = productRepsitory;
                _categoryRepository = categoryRepository;
            }

            public async Task<PagedResponse<IEnumerable<GetAllProductsViewModel>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
            {
                var list = (from p in _productRepsitory.Entities
                            join c in _categoryRepository.Entities on p.CategoryId equals c.Id
                            where (string.IsNullOrEmpty(query.ProductName) || p.Name.ToLower().Contains(query.ProductName.ToLower()))
                            //&& (!query.FromPrice.HasValue || p.Price >= query.FromPrice.Value)
                            //&& (!query.ToPrice.HasValue || p.Price <= query.ToPrice.Value)
                            && (!query.FromRate.HasValue || p.Rate >= query.FromRate.Value)
                            && (!query.ToRate.HasValue || p.Rate <= query.ToRate.Value)
                            && (string.IsNullOrEmpty(query.CategoryName) || c.Name.ToLower().Contains(query.CategoryName.ToLower()))
                            select new GetAllProductsViewModel()
                            {
                                Id = p.Id,
                                ProductName = p.Name,
                                CategoryName = c.Name,
                                Barcode = p.Barcode,
                                Description = p.Description,
                                //Price = p.Price,
                                //Quantity = p.Quantity,
                                Rate = p.Rate
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
                var rs = await list.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetAllProductsViewModel>>(list, query.PageNumber, query.PageSize, total));
            }
        }
    }
}