using Application.Features.OrderFeatures.Queries.GetAllOrdersQuery;
using Application.Filter;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.OrderFeatures.Queries.GetOrdersByUserIdQuery
{
    public class GetOrdersByUserIdQuery : IRequest<PagedResponse<IEnumerable<GetOrdersByUserIdVM>>>
    {
        public PaginationFilter Filter { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }

        internal class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, PagedResponse<IEnumerable<GetOrdersByUserIdVM>>>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;

            public GetOrdersByUserIdQueryHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<PagedResponse<IEnumerable<GetOrdersByUserIdVM>>> Handle(GetOrdersByUserIdQuery query, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var validFilter = new PaginationFilter(query.Filter.PageNumber, query.Filter.PageSize);
                var list = (from o in _context.Orders
                            join u in _context.Users on o.UserId equals u.Id
                            where u.Id == userId
                            && (!query.FromPrice.HasValue || o.TotalPrice >= query.FromPrice.Value)
                            && (!query.ToPrice.HasValue || o.TotalPrice <= query.ToPrice.Value)
                            && (!query.FromDate.HasValue || o.CreatedOn <= query.FromDate.Value)
                            && (!query.ToDate.HasValue || o.CreatedOn >= query.ToDate.Value)
                            select new GetOrdersByUserIdVM()
                            {
                                OrderId = o.Id,
                                TotalPrice = o.TotalPrice,
                                CreatedOn = o.CreatedOn
                            });
                list = query.Order switch
                {
                    "asc" => query.SortBy switch
                    {
                        "Price" => list.OrderBy(x => x.TotalPrice),
                        "Date" => list.OrderBy(x => x.CreatedOn)
                    },
                    "desc" => query.SortBy switch
                    {
                        "Price" => list.OrderByDescending(x => x.TotalPrice),
                        "Date" => list.OrderByDescending(x => x.CreatedOn)
                    },
                    _ => list
                };
                var total = list.Count();
                var rs = await list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetOrdersByUserIdVM>>(list, validFilter.PageNumber, validFilter.PageSize, total));
            }
        }
    }
}