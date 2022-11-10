using Application.Dtos.Products;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Response<GetProductByIdViewModel>>
    {
        public int Id { get; set; }

        internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdViewModel>>
        {
            private readonly IProductRepository _productRepsitory;
            private readonly ICategoryRepository _categoryRepository;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IImageProductRepository _imageProductRepository;

            public GetProductByIdQueryHandler(IProductRepository productRepsitory, ICategoryRepository categoryRepository, IProductDetailRepository productDetailRepository, IImageProductRepository imageProductRepository)
            {
                _productRepsitory = productRepsitory;
                _categoryRepository = categoryRepository;
                _productDetailRepository = productDetailRepository;
                _imageProductRepository = imageProductRepository;
            }

            public async Task<Response<GetProductByIdViewModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await (from p in _productRepsitory.Entities
                                    join c in _categoryRepository.Entities
                                    on p.CategoryId equals c.Id
                                    where p.Id == request.Id && p.IsDeleted == false
                                    select new GetProductByIdViewModel()
                                    {
                                        Id = p.Id,
                                        ProductName = p.Name,
                                        CategoryName = c.Name,
                                        Barcode = p.Barcode,
                                        Description = p.Description,
                                        //Price = p.Price,
                                        //Quantity = p.Quantity,
                                        Images = (from p in _productRepsitory.Entities
                                                  join ip in _imageProductRepository.Entities
                                                  on p.Id equals ip.ProductId into productImg
                                                  from lj in productImg.DefaultIfEmpty()
                                                  where p.Id == request.Id
                                                  select new ImageProductDto
                                                  {
                                                      Url = lj.Url
                                                  }).ToList(),
                                        ProductDetails = (
                                                  from p in _productRepsitory.Entities
                                                  join pd in _productDetailRepository.Entities
                                                  on p.Id equals pd.ProductId into productdetail
                                                  from lj in productdetail.DefaultIfEmpty()
                                                  where lj.ProductId == request.Id
                                                  select new ProductDetailDto
                                                  {
                                                      ColorName = lj.Color,
                                                      Quantity = lj.Quantity,
                                                      ImageUrl = lj.ImageUrl,
                                                      Price = lj.Price
                                                  }).ToList()
                                    }).FirstOrDefaultAsync();
                if (result == null) throw new ApiException("Product not found");
                return new Response<GetProductByIdViewModel>(result);
            }
        }
    }
}