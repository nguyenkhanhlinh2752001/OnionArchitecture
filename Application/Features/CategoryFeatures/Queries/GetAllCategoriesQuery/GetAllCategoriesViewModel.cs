namespace Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ProductAmount { get; set; } = 0;
        public DateTime? CreatedOn { get; set; }
    }
}