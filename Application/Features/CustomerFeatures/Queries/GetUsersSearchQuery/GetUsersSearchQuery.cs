using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.CustomerFeatures.Queries.GetUsersSearchQuery
{
    public class GetUsersSearchQuery : IRequest<IEnumerable<GetUsersSearchQueryVM>>
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        public class GetUsersSearchQueryHandler : IRequestHandler<GetUsersSearchQuery, IEnumerable<GetUsersSearchQueryVM>>
        {
            private readonly ApplicationDbContext _context;

            public GetUsersSearchQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetUsersSearchQueryVM>> Handle(GetUsersSearchQuery query, CancellationToken cancellationToken)
            {
                var list = await (from u in _context.Users
                                  where (string.IsNullOrEmpty(query.FullName) || u.FullName.ToLower().Contains(query.FullName.ToLower()))
                                  && (string.IsNullOrEmpty(query.PhoneNumber) || u.PhoneNumber.Contains(query.PhoneNumber))
                                  && (string.IsNullOrEmpty(query.Address) || u.Address.ToLower().Contains(query.Address.ToLower()))
                                  && (!query.CreatedFrom.HasValue || u.CreatedOn.Value.Date >= query.CreatedFrom.Value.Date)
                                  && (!query.CreatedTo.HasValue || u.CreatedOn.Value.Date <= query.CreatedTo.Value.Date)
                                  && (!query.IsActive.HasValue || u.IsActive.Equals(query.IsActive))
                                  select new GetUsersSearchQueryVM
                                  {
                                      Id = u.Id,
                                      FullName = u.FullName,
                                      Address = u.Address,
                                      PhoneNumber = u.PhoneNumber,
                                      CreatedOn = u.CreatedOn.Value,
                                      IsActive = u.IsActive
                                  }).ToListAsync();
                return list;
            }
        }
    }
}