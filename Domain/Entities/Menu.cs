namespace Domain.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ParentId { get; set; }
    }
}