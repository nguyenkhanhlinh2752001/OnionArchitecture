namespace Persistence.Services
{
    public interface ICurrentUserService
    {
        string Id { get; }
        string Username { get; }
        string Role { get; }
    }
}