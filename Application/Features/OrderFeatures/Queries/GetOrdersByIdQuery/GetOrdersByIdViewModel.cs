using Application.ViewModels;

namespace Application.Features.OrderFeatures.Queries.GetOrdersByIdQuery
{
    public class GetOrdersByIdViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<OrderDetailVM>? OrderDetails { get; set; }
    }
}