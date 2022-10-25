using Application.Filter;

namespace Application.Features.UserFeatures.Queries.GetAllUsersQuery
{
    public class GetAllUsersParameter : PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}