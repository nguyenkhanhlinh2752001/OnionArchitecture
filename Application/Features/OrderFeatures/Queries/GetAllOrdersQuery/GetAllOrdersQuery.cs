using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Dynamic.Core;

namespace Application.Features.OrderFeatures.Queries.GetAllOrdersQuery
{
    public class GetAllOrdersQuery : IRequest<PagedResponse<IEnumerable<GetAllOrdersVM>>>
    {
        public string? OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? CreatedOn { get; set; }

        internal class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PagedResponse<IEnumerable<GetAllOrdersVM>>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IOrderRespository _orderRespository;

            public GetAllOrdersQueryHandler(ApplicationDbContext context, IOrderRespository orderRespository)
            {
                _context = context;
                _orderRespository = orderRespository;
            }

            public async Task<PagedResponse<IEnumerable<GetAllOrdersVM>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
            {
                var query = (from o in _orderRespository.Entities
                             join u in _context.Users on o.UserId equals u.Id
                             where (string.IsNullOrEmpty(request.UserName) || u.UserName.ToLower().Contains(request.UserName.ToLower()))
                             && (string.IsNullOrEmpty(request.PhoneNumber) || u.PhoneNumber.Contains(request.PhoneNumber))
                             && (!request.TotalPrice.HasValue || o.TotalPrice == request.TotalPrice.Value)
                             && (!request.CreatedOn.HasValue || o.CreatedOn == request.CreatedOn.Value)
                             select new GetAllOrdersVM()
                             {
                                 UserId = u.Id,
                                 UserName = u.UserName,
                                 PhoneNumber = u.PhoneNumber,
                                 TotalPrice = o.TotalPrice,
                                 CreatedOn = o.CreatedOn
                             });
                var data = query.OrderBy(request.OrderBy!);
                var total = data.Count();
                var rs = await data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetAllOrdersVM>>(rs, request.PageNumber, request.PageSize, total));
            }
        }
    }
}