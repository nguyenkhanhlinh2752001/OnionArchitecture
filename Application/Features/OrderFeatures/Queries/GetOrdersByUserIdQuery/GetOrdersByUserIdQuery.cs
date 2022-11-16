using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Services;
using System.Linq.Dynamic.Core;

namespace Application.Features.OrderFeatures.Queries.GetOrdersByUserIdQuery
{
    public class GetOrdersByUserIdQuery : IRequest<PagedResponse<IEnumerable<GetOrdersByUserIdVM>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? CreatedOn { get; set; }

        internal class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, PagedResponse<IEnumerable<GetOrdersByUserIdVM>>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IOrderRespository _orderRespository;
            private readonly ICurrentUserService _currentUserService;

            public GetOrdersByUserIdQueryHandler(ApplicationDbContext context, ICurrentUserService currentUserService, IOrderRespository orderRespository)
            {
                _context = context;
                _currentUserService = currentUserService;
                _orderRespository = orderRespository;
            }

            public async Task<PagedResponse<IEnumerable<GetOrdersByUserIdVM>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var query = (from o in _orderRespository.Entities
                             join u in _context.Users on o.UserId equals u.Id
                             where u.Id == userId
                             && (!request.TotalPrice.HasValue || o.TotalPrice == request.TotalPrice.Value)
                             && (!request.CreatedOn.HasValue || o.CreatedOn == request.CreatedOn.Value)
                             select new GetOrdersByUserIdVM()
                             {
                                 OrderId = o.Id,
                                 TotalPrice = o.TotalPrice,
                                 CreatedOn = o.CreatedOn
                             });

                var data = query.OrderBy(request.OrderBy!);
                var total = data.Count();
                var rs = await data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetOrdersByUserIdVM>>(rs, request.PageNumber, request.PageSize, total));
            }
        }
    }
}