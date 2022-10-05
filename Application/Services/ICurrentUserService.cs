namespace Application.Services
{
    public interface ICurrentUserService
    {
        string Username { get; }
        string RoleId { get; }
    }
}