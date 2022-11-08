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
            private readonly IProductRepsitory _productRepsitory;
            private readonly ApplicationDbContext _context;

            public GetReviewsByProductIdQueryHandler(IReviewRepository reviewRepository, IProductRepsitory productRepsitory, ApplicationDbContext context)
            {
                _reviewRepository = reviewRepository;
                _productRepsitory = productRepsitory;
                _context = context;
            }

            public async Task<PagedResponse<IEnumerable<GetReviewsByProductIdViewModel>>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
            {
                var list = from p in _productRepsitory.Entities
                           join r in _reviewRepository.Entities
                           on p.Id equals r.ProductId
                           join u in _context.Users
                           on r.UserId equals u.Id
                           where r.ProductId == request.ProductId
                           select new GetReviewsByProductIdViewModel
                           {
                               Id = p.Id,
                               Content = r.Content,
                               Rate = r.Rate,
                               Title = r.Title,
                               UserName = u.UserName,
                               CreatedOn = r.CreatedOn
                           };
                var data = list.OrderBy(request.OrderBy);
                var total = data.Count();
                var rs = await data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetReviewsByProductIdViewModel>>(data, request.PageNumber, request.PageSize, total));
            }
        }
    }
}