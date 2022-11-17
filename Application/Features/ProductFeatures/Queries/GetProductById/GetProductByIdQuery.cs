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
        public int ProductId { get; set; }

        internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdViewModel>>
        {
            private readonly IProductRepository _productRepsitory;
            private readonly ICategoryRepository _categoryRepository;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IImageProductRepository _imageProductRepository;
            private readonly IReviewRepository _reviewRepository;
            private readonly IOrderDetailRepository _orderDetailRepository;

            public GetProductByIdQueryHandler(IProductRepository productRepsitory, ICategoryRepository categoryRepository, IProductDetailRepository productDetailRepository, IImageProductRepository imageProductRepository, IReviewRepository reviewRepository, IOrderDetailRepository orderDetailRepository)
            {
                _productRepsitory = productRepsitory;
                _categoryRepository = categoryRepository;
                _productDetailRepository = productDetailRepository;
                _imageProductRepository = imageProductRepository;
                _reviewRepository = reviewRepository;
                _orderDetailRepository = orderDetailRepository;
            }

            public async Task<Response<GetProductByIdViewModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                var totalReview = (from r in _reviewRepository.Entities
                                   join pd in _productDetailRepository.Entities
                                     on r.ProductDetailId equals pd.Id into leftJoinProductDetail
                                   from productDetail in leftJoinProductDetail.DefaultIfEmpty()
                                   where r.ProductDetailId == productDetail.Id
                                   && productDetail.ProductId == request.ProductId
                                   select r).Count();

                var avgRate = (from r in _reviewRepository.Entities
                               join pd in _productDetailRepository.Entities
                                 on r.ProductDetailId equals pd.Id into leftJoinProductDetail
                               from productDetail in leftJoinProductDetail.DefaultIfEmpty()
                               where r.ProductDetailId == productDetail.Id
                               && productDetail.ProductId == request.ProductId
                               select r).Average(e => e.Rate);

                var saleAmount = (from od in _orderDetailRepository.Entities
                                  join pd in _productDetailRepository.Entities
                                  on od.ProductDetailId equals pd.Id
                                  where pd.ProductId == request.ProductId
                                  select od).Sum(e => e.Quantity);

                var result = await (from p in _productRepsitory.Entities
                                    join c in _categoryRepository.Entities
                                    on p.CategoryId equals c.Id into leftjoinCategory
                                    from category in leftjoinCategory.DefaultIfEmpty()
                                    join pd in _productDetailRepository.Entities
                                    on p.Id equals pd.ProductId into leftjoinProductDetail
                                    from prductDetail in leftjoinProductDetail.DefaultIfEmpty()
                                    where p.Id == request.ProductId && p.IsDeleted == false
                                    select new GetProductByIdViewModel()
                                    {
                                        Id = p.Id,
                                        ProductName = p.Name,
                                        CategoryName = category.Name,
                                        Barcode = p.Barcode,
                                        Description = p.Description,
                                        TotalReview = totalReview,
                                        AvgRate = Math.Round((decimal)avgRate, 1),
                                        SaleAmount = saleAmount,
                                        Images = (from ip in _imageProductRepository.Entities
                                                  where ip.ProductId == p.Id
                                                  select new ImageProductDto
                                                  {
                                                      Url = ip.Url
                                                  }).ToList(),
                                        ProductDetails = (
                                                  from pd in _productDetailRepository.Entities
                                                  where pd.ProductId == p.Id
                                                  select new ProductDetailDto
                                                  {
                                                      Color = pd.Color,
                                                      Quantity = pd.Quantity,
                                                      ImageUrl = pd.ImageUrl,
                                                      Price = pd.Price
                                                  }).ToList()
                                    }).FirstOrDefaultAsync();
                if (result == null) throw new ApiException("Product not found");
                return new Response<GetProductByIdViewModel>(result);
            }
        }
    }
}