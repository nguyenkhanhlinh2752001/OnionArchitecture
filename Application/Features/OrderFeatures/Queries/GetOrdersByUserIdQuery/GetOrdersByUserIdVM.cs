namespace Application.Features.OrderFeatures.Queries.GetOrdersByUserIdQuery
{
    public class GetOrdersByUserIdVM
    {
        public int OrderId { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}