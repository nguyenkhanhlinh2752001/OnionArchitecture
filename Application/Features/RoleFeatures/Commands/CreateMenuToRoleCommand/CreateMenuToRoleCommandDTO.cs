namespace Application.Features.RoleFeatures.Commands.CreateMenuToRoleCommand
{
    public class CreateMenuToRoleCommandDTO
    {
        public int MenuId { get; set; }
        public bool CanAccess { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}