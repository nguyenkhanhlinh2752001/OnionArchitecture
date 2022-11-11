using Application.Dtos.Reviews;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Dynamic.Core;

namespace Application.Features.ProductFeatures.Queries.GetReviewsByProductId
{
    public class GetReviewsByProductIdQuery : IRequest<PagedResponse<IEnumerable<GetReviewsByProductIdViewModel>>>
    {
        public int ProductId { get; set; }
        public string? OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? Rate { get; set; }

        internal class GetReviewsByProductIdQueryHandler : IRequestHandler<GetReviewsByProductIdQuery, PagedResponse<IEnumerable<GetReviewsByProductIdViewModel>>>
        {
            private readonly IReviewRepository _reviewRepository;
            private readonly IImageReviewRepository _imageReviewRepository;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly ApplicationDbContext _context;

            public GetReviewsByProductIdQueryHandler(IReviewRepository reviewRepository, ApplicationDbContext context, IImageReviewRepository imageReviewRepository, IProductDetailRepository productDetailRepository)
            {
                _reviewRepository = reviewRepository;
                _imageReviewRepository = imageReviewRepository;
                _context = context;
                _productDetailRepository = productDetailRepository;
            }

            public async Task<PagedResponse<IEnumerable<GetReviewsByProductIdViewModel>>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
            {
                var listProductDetailid = await _productDetailRepository.Entities.Where(x => x.ProductId == request.ProductId).Select(x => x.Id).ToListAsync();
                if (listProductDetailid == null) throw new ApiException("");

                var query = _reviewRepository.Entities.Where(x => listProductDetailid.Contains(x.ProductDetailId))
                    .Select(x => new GetReviewsByProductIdViewModel
                    {
                        Rate = x.Rate,
                        Title = x.Title,
                        CreatedOn = x.CreatedOn,
                        Images = _imageReviewRepository.Entities.Where(a => a.ReviewId == x.Id)
                        .Select(a => new ImageReviewDto
                        {
                            Url = a.Url
                        }).ToList()
                    });

                var query2 = (from r in _reviewRepository.Entities
                              join u in _context.Users
                                on r.UserId equals u.Id into leftJoinUser
                              from user in leftJoinUser.DefaultIfEmpty()
                              join pd in _productDetailRepository.Entities
                                on r.ProductDetailId equals pd.Id into leftJoinProductDetail
                              from productDetail in leftJoinProductDetail.DefaultIfEmpty()
                              where r.ProductDetailId == productDetail.Id
                              && productDetail.ProductId == request.ProductId
                              && (!request.Rate.HasValue || r.Rate == request.Rate.Value)
                              select new GetReviewsByProductIdViewModel
                              {
                                  UserName = user.UserName,
                                  ProductDetailId = productDetail.Id,
                                  Color = productDetail.Color,
                                  Content = r.Content,
                                  Rate = r.Rate,
                                  CreatedOn = r.CreatedOn,
                                  Title = r.Title,
                                  Images = (from ir in _imageReviewRepository.Entities
                                            where r.Id == ir.ReviewId
                                            select new ImageReviewDto
                                            {
                                                Url = ir.Url
                                            }).ToList()
                              });

                var data = query2.OrderBy(request.OrderBy);

                var total = query2.Count();
                var rs = await query2.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetReviewsByProductIdViewModel>>(rs, request.PageNumber, request.PageSize, total));
            }
        }
    }
}