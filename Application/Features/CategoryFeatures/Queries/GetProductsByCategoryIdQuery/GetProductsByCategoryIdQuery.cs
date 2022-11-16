using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CategoryFeatures.Queries.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdQuery : IRequest<PagedResponse<IEnumerable<GetProductsByCategoryIdViewModel>>>
    {
        public int CategoryId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }

        internal class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, PagedResponse<IEnumerable<GetProductsByCategoryIdViewModel>>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IProductRepository _productRepsitory;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IOrderRespository _orderRespository;
            private readonly IOrderDetailRepository _orderDetailRepository;

            public GetProductsByCategoryIdQueryHandler(ICategoryRepository categoryRepository, IProductRepository productRepsitory, IProductDetailRepository productDetailRepository, IOrderRespository orderRespository, IOrderDetailRepository orderDetailRepository)
            {
                _categoryRepository = categoryRepository;
                _productRepsitory = productRepsitory;
                _productDetailRepository = productDetailRepository;
                _orderRespository = orderRespository;
                _orderDetailRepository = orderDetailRepository;
            }

            public async Task<PagedResponse<IEnumerable<GetProductsByCategoryIdViewModel>>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
            {
                var list = (from p in _productRepsitory.Entities
                            join ct in _categoryRepository.Entities
                            on p.CategoryId equals ct.Id
                            where p.CategoryId == request.CategoryId
                            select new GetProductsByCategoryIdViewModel
                            {
                                ProductName = p.Name,
                                Rate = p.Rate
                            });

                list = request.Order switch
                {
                    "asc" => request.SortBy switch
                    {
                        "ProductName" => list.OrderBy(x => x.ProductName),
                    },
                    "desc" => request.SortBy switch
                    {
                        "ProductName" => list.OrderByDescending(x => x.ProductName),
                    },
                    _ => list
                };
                var total = list.Count();
                var rs = await list.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return new PagedResponse<IEnumerable<GetProductsByCategoryIdViewModel>>(list, request.PageNumber, request.PageSize, total);
            }
        }
    }
}