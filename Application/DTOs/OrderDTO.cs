namespace Application.DTOs
{
    public class OrderDTO : BaseDTO
    {
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<OrderDetailDTO> OrderDetails { get; set; }
    }
}