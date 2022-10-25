using System.ComponentModel;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DeleledOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? LastEditBy { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}