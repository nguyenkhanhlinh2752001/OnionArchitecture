namespace Application.Features.OrderFeatures.Queries.GetAllOrderByCreatedDateAtQuery
{
    public class GetAllOrdersByCreatedDateAtViewModel
    {
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}