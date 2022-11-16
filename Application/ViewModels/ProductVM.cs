namespace Application.ViewModels
{
    public class ProductVM : BaseVM
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public decimal? Rate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}