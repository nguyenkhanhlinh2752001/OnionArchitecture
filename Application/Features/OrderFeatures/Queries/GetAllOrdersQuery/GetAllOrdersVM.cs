namespace Application.Features.OrderFeatures.Queries.GetAllOrdersQuery
{
    public class GetAllOrdersVM
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}