using Domain.Common;

namespace Domain.Entities
{
    public class RolePermisson : BaseEntity
    {
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public bool CanAccess { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}