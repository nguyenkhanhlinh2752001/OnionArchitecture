using Application.Filter;

namespace Application.Features.UserFeatures.Queries.GetAllUsersQuery
{
    public class GetAllUsersQueryVM
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}