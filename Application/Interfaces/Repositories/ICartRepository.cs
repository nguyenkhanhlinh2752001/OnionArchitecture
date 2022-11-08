using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICartRepository : IRepositoryAsync<Cart, int>
    {
    }
}