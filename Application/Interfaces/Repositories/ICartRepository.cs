using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICartRepository : RepositoryAsync<Cart, int>
    {
    }
}