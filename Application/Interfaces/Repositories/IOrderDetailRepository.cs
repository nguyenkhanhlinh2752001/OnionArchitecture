using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderDetailRepository : IRepositoryAsync<OrderDetail, int>
    {
    }
}