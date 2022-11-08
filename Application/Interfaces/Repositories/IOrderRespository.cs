using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRespository : IRepositoryAsync<Order, int>
    {
    }
}