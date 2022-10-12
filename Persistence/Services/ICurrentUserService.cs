namespace Persistence.Services
{
    public interface ICurrentUserService
    {
        string Id { get; }
        string Email { get; }
        string Role { get; }
    }
}