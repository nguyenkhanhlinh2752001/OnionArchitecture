using Application.ViewModels;
using Application.Filter;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Application.Interfaces.Repositories;

namespace Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQuery : IRequest<PagedResponse<IEnumerable<GetAllCategoriesVM>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }
        public string? Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CreatedBy { get; set; }

        internal class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PagedResponse<IEnumerable<GetAllCategoriesVM>>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IProductRepsitory _productRepsitory;

            public GetAllCategoriesQueryHandler( ICategoryRepository categoryRepository, IProductRepsitory productRepsitory)
            {
                _categoryRepository = categoryRepository;
                _productRepsitory = productRepsitory;
            }

            public async Task<PagedResponse<IEnumerable<GetAllCategoriesVM>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
            {
                var list = (from c in _categoryRepository.Entities
                            where (string.IsNullOrEmpty(query.Name) || c.Name.ToLower().Contains(query.Name.ToLower()))
                            && (string.IsNullOrEmpty(query.CreatedBy) || c.CreatedBy.Contains(query.CreatedBy))
                            && (!query.FromDate.HasValue || c.CreatedOn <= query.FromDate.Value)
                            && (!query.ToDate.HasValue || c.CreatedOn >= query.ToDate.Value)
                            select new GetAllCategoriesVM()
                            {
                                Id = c.Id,
                                Name = c.Name,
                                CreatedBy = c.CreatedBy,
                                CreatedOn = c.CreatedOn,
                                Products = (from p in _productRepsitory.Entities
                                            join v in _categoryRepository.Entities
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
                        "CreatedBy" => list.OrderBy(x => x.CreatedBy),
                        "CreatedOn"=>list.OrderBy(x=>x.CreatedOn)
                    },
                    "desc" => query.SortBy switch
                    {
                        "Name" => list.OrderByDescending(x => x.Name),
                        "CreatedBy" => list.OrderByDescending(x => x.CreatedBy),
                        "CreatedOn" => list.OrderByDescending(x => x.CreatedOn)
                    },
                    _ => list
                };
                var total = list.Count();
                var rs = await list.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

                return (new PagedResponse<IEnumerable<GetAllCategoriesVM>>(list, query.PageNumber, query.PageSize, total));
            }
        }
    }
}