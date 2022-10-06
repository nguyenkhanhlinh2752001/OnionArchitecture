namespace Domain.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ParentId { get; set; }
    }
}