using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Queries.GetAllOrdersQuery
{
    public class GetAllOrdersQuery : IRequest<PagedResponse<IEnumerable<GetAllOrdersVM>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? TotalPriceFrom { get; set; }
        public decimal? TotalPriceTo { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        internal class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PagedResponse<IEnumerable<GetAllOrdersVM>>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IOrderRespository _orderRespository;

            public GetAllOrdersQueryHandler(ApplicationDbContext context, IOrderRespository orderRespository)
            {
                _context = context;
                _orderRespository = orderRespository;
            }

            public async Task<PagedResponse<IEnumerable<GetAllOrdersVM>>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken)
            {
                var list = (from o in _orderRespository.Entities
                            join u in _context.Users on o.UserId equals u.Id
                            where (string.IsNullOrEmpty(query.UserName) || u.UserName.ToLower().Contains(query.UserName.ToLower()))
                            && (string.IsNullOrEmpty(query.PhoneNumber) || u.PhoneNumber.Contains(query.PhoneNumber))
                            && (!query.TotalPriceFrom.HasValue || o.TotalPrice >= query.TotalPriceFrom.Value)
                            && (!query.TotalPriceTo.HasValue || o.TotalPrice <= query.TotalPriceTo.Value)
                            && (!query.CreatedFrom.HasValue || o.CreatedOn <= query.CreatedFrom.Value)
                            && (!query.CreatedTo.HasValue || o.CreatedOn >= query.CreatedTo.Value)
                            select new GetAllOrdersVM()
                            {
                                UserId = u.Id,
                                UserName = u.UserName,
                                PhoneNumber = u.PhoneNumber,
                                TotalPrice = o.TotalPrice,
                                CreatedDate = o.CreatedOn
                            });
                list = query.Order switch
                {
                    "asc" => query.SortBy switch
                    {
                        "Username" => list.OrderBy(x => x.UserName),
                        "Price" => list.OrderBy(x => x.TotalPrice),
                        "Date" => list.OrderBy(x => x.CreatedDate)
                    },
                    "desc" => query.SortBy switch
                    {
                        "Username" => list.OrderByDescending(x => x.UserName),
                        "Price" => list.OrderByDescending(x => x.TotalPrice),
                        "Date" => list.OrderByDescending(x => x.CreatedDate)
                    },
                    _ => list
                };
                var total = list.Count();
                var rs = await list.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetAllOrdersVM>>(list, query.PageNumber, query.PageSize, total));
            }
        }
    }
}