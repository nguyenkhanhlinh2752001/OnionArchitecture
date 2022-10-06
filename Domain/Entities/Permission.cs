namespace Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string RoleId { get; set; }
        public bool CanAccess { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}