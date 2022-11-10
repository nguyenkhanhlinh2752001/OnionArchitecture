using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CategoryFeatures.Queries.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdQuery : IRequest<PagedResponse<IEnumerable<GetProductsByCategoryIdVM>>>
    {
        public int CategoryId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }

        internal class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, PagedResponse<IEnumerable<GetProductsByCategoryIdVM>>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IProductRepository _productRepsitory;

            public GetProductsByCategoryIdQueryHandler(ICategoryRepository categoryRepository, IProductRepository productRepsitory)
            {
                _categoryRepository = categoryRepository;
                _productRepsitory = productRepsitory;
            }

            public async Task<PagedResponse<IEnumerable<GetProductsByCategoryIdVM>>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
            {
                var list = (from p in _productRepsitory.Entities
                            join ct in _categoryRepository.Entities
                            on p.CategoryId equals ct.Id
                            where p.CategoryId == request.CategoryId
                            select new GetProductsByCategoryIdVM
                            {
                                ProductName = p.Name,
                                Description = p.Description,
                                Rate = p.Rate
                            });

                list = request.Order switch
                {
                    "asc" => request.SortBy switch
                    {
                        "ProductName" => list.OrderBy(x => x.ProductName),
                        "Description" => list.OrderBy(x => x.Description),
                        "Rate" => list.OrderBy(x => x.Rate),
                        "Price" => list.OrderBy(x => x.Price),
                        "Quantity" => list.OrderBy(x => x.Quantity),
                    },
                    "desc" => request.SortBy switch
                    {
                        "ProductName" => list.OrderByDescending(x => x.ProductName),
                        "Description" => list.OrderByDescending(x => x.Description),
                        "Rate" => list.OrderByDescending(x => x.Rate),
                        "Price" => list.OrderByDescending(x => x.Price),
                        "Quantity" => list.OrderByDescending(x => x.Quantity),
                    },
                    _ => list
                };
                var total = list.Count();
                var rs = await list.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return new PagedResponse<IEnumerable<GetProductsByCategoryIdVM>>(list, request.PageNumber, request.PageSize, total);
            }
        }
    }
}