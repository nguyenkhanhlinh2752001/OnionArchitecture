using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderDetailRepository : RepositoryAsync<OrderDetail, int>
    {
    }
}