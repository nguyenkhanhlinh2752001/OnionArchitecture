using System.ComponentModel;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? DeleledDate { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}