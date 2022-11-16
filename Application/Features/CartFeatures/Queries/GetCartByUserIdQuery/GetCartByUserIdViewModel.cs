namespace Application.Features.CartFeatures.Queries.GetCartByUserIdQuery
{
    public class GetCartByUserIdViewModel
    {
        public int CartDetailId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal ToTal { get; set; }
    }
}