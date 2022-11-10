using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRespository : RepositoryAsync<Order, int>
    {
    }
}