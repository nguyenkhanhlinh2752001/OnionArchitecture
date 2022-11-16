using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQuery : IRequest<PagedResponse<IEnumerable<GetAllCategoriesViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
        public string? Name { get; set; }

        internal class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PagedResponse<IEnumerable<GetAllCategoriesViewModel>>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IProductRepository _productRepsitory;
            private readonly IProductDetailRepository _productDetailRepository;

            public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IProductRepository productRepsitory, IProductDetailRepository productDetailRepository)
            {
                _categoryRepository = categoryRepository;
                _productRepsitory = productRepsitory;
                _productDetailRepository = productDetailRepository;
            }

            public async Task<PagedResponse<IEnumerable<GetAllCategoriesViewModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
            {
                var list = (from c in _categoryRepository.Entities
                            where (string.IsNullOrEmpty(request.Name) || c.Name.ToLower().Contains(request.Name.ToLower()))
                            select new GetAllCategoriesViewModel()
                            {
                                Id = c.Id,
                                Name = c.Name,
                                CreatedOn = c.CreatedOn,
                                ProductAmount = (from p in _productRepsitory.Entities
                                                 where p.CategoryId == c.Id
                                                 select p).Count()
                            });
                var data = list.OrderBy(request.OrderBy!);
                var total = data.Count();
                var rs = await data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetAllCategoriesViewModel>>(rs, request.PageNumber, request.PageSize, total));
            }
        }
    }
}