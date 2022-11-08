using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Response<GetProductByIdViewModel>>
    {
        public int Id { get; set; }

        internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdViewModel>>
        {
            private readonly IProductRepsitory _productRepsitory;
            private readonly ICategoryRepository _categoryRepository;

            public GetProductByIdQueryHandler(IProductRepsitory productRepsitory, ICategoryRepository categoryRepository)
            {
                _productRepsitory = productRepsitory;
                _categoryRepository = categoryRepository;
            }

            public async Task<Response<GetProductByIdViewModel>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
            {
                var result = (from p in _productRepsitory.Entities
                              join c in _categoryRepository.Entities on p.CategoryId equals c.Id
                              where p.Id == query.Id && p.IsDeleted == false
                              select new GetProductByIdViewModel()
                              {
                                  Id = p.Id,
                                  ProductName = p.Name,
                                  CategoryName = c.Name,
                                  Barcode = p.Barcode,
                                  Description = p.Description,
                                  Price = p.Price,
                                  Quantity = p.Quantity,
                              }).FirstOrDefault();
                return new Response<GetProductByIdViewModel>(result);
            }
        }
    }
}