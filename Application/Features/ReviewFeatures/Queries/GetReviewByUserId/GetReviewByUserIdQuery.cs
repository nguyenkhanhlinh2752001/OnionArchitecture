using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Services;
using System.Linq.Dynamic.Core;

namespace Application.Features.ReviewFeatures.Queries.GetReviewByUserId
{
    public class GetReviewByUserIdQuery : IRequest<PagedResponse<IEnumerable<GetReviewByUserIdViewModel>>>
    {
        public string? OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? ProductName { get; set; }
        public int? Rate { get; set; }

        internal class GetReviewByUserIdQueryHandler : IRequestHandler<GetReviewByUserIdQuery, PagedResponse<IEnumerable<GetReviewByUserIdViewModel>>>
        {
            private readonly IReviewRepository _reviewRepository;
            private readonly ICurrentUserService _currentUserService;
            private readonly IProductRepository _productRepsitory;

            public GetReviewByUserIdQueryHandler(IReviewRepository reviewRepository, ICurrentUserService currentUserService, IProductRepository productRepsitory)
            {
                _reviewRepository = reviewRepository;
                _currentUserService = currentUserService;
                _productRepsitory = productRepsitory;
            }

            public async Task<PagedResponse<IEnumerable<GetReviewByUserIdViewModel>>> Handle(GetReviewByUserIdQuery request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var list = (from p in _productRepsitory.Entities
                            join r in _reviewRepository.Entities
                            on p.Id equals r.ProductDetailId
                            where r.UserId == userId
                            && r.IsCheck == true
                            && (string.IsNullOrEmpty(request.ProductName) || p.Name.ToLower().Contains(request.ProductName.ToLower()))
                            && (!request.Rate.HasValue || r.Rate == request.Rate.Value)
                            select new GetReviewByUserIdViewModel
                            {
                                ProductId = p.Id,
                                ProductName = p.Name,
                                Content = r.Content,
                                Rate = r.Rate,
                                Title = r.Title,
                                CreatedOn = r.CreatedOn
                            });
                var data = list.OrderBy(request.OrderBy!);
                var total = data.Count();
                var rs = await data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetReviewByUserIdViewModel>>(rs, request.PageNumber, request.PageSize, total));
            }
        }
    }
}