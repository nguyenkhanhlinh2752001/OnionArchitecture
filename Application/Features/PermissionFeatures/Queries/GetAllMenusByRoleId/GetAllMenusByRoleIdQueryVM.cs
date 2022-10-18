namespace Application.Features.PermissionFeatures.Queries.GetAllMenusByRoleId
{
    public class GetAllMenusByRoleIdQueryVM
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public bool CanAccess { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}