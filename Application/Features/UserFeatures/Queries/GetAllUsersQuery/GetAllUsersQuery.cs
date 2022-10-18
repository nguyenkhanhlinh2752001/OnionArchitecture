using Application.Filter;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.UserFeatures.Queries.GetAllUsersQuery
{
    public class GetAllUsersQuery : IRequest<PagedResponse<IEnumerable<GetAllUsersQueryVM>>>
    {
        public PaginationFilter Filter { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResponse<IEnumerable<GetAllUsersQueryVM>>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllUsersQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PagedResponse<IEnumerable<GetAllUsersQueryVM>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
            {
                var validFilter = new PaginationFilter(query.Filter.PageNumber, query.Filter.PageSize);
                var list = (from u in _context.Users
                            where (string.IsNullOrEmpty(query.FullName) || u.FullName.ToLower().Contains(query.FullName.ToLower()))
                            && (string.IsNullOrEmpty(query.PhoneNumber) || u.PhoneNumber.Contains(query.PhoneNumber))
                            && (string.IsNullOrEmpty(query.Address) || u.Address.ToLower().Contains(query.Address.ToLower()))
                            && (!query.CreatedFrom.HasValue || u.CreatedOn.Value.Date >= query.CreatedFrom.Value.Date)
                            && (!query.CreatedTo.HasValue || u.CreatedOn.Value.Date <= query.CreatedTo.Value.Date)
                            && (!query.IsActive.HasValue || u.IsActive.Equals(query.IsActive))
                            select new GetAllUsersQueryVM
                            {
                                Id = u.Id,
                                FullName = u.FullName,
                                Address = u.Address,
                                PhoneNumber = u.PhoneNumber,
                                CreatedOn = u.CreatedOn.Value,
                                IsActive = u.IsActive
                            });
                list = query.Order switch
                {
                    "asc" => query.SortBy switch
                    {
                        "Name" => list.OrderBy(x => x.FullName),
                        "Address" => list.OrderBy(x => x.Address),
                        "PhoneNumber" => list.OrderBy(x => x.PhoneNumber),
                        "CreatedOn" => list.OrderBy(x => x.CreatedOn)
                    },
                    "desc" => query.SortBy switch
                    {
                        "Name" => list.OrderByDescending(x => x.FullName),
                        "Address" => list.OrderByDescending(x => x.Address),
                        "PhoneNumber" => list.OrderByDescending(x => x.PhoneNumber),
                        "CreatedOn" => list.OrderByDescending(x => x.CreatedOn)
                    },
                    _ => list
                };
                var total = list.Count();
                var rs = await list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetAllUsersQueryVM>>(list, validFilter.PageNumber, validFilter.PageSize, total));
            }
        }
    }
}