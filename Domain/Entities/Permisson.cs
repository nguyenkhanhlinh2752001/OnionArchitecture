using Domain.Common;

namespace Domain.Entities
{
    public class Permisson : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }
        public int Order { get; set; }
    }
}