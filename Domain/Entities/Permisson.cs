using Domain.Common;

namespace Domain.Entities
{
    public class Permisson : BaseEntity
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public bool CanAccess { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

    }
}