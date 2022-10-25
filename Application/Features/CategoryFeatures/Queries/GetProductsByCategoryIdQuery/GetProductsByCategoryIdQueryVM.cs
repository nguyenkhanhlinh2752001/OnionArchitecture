using Application.ViewModels;

namespace Application.Features.CategoryFeatures.Queries.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdQueryVM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductVM>? Products { get; set; }
    }
}