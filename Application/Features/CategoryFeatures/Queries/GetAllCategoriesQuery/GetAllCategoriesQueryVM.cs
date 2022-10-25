using Application.ViewModels;

namespace Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQueryVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public List<ProductVM>? Products { get; set; }
    }
}