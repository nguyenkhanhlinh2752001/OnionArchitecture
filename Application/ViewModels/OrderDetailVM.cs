namespace Application.ViewModels
{
    public class OrderDetailVM : BaseVM
    {
        public string ProductName { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}