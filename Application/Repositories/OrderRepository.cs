using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Application.Repositories
{
    public class OrderRepository : RepositoryAsync<Order, int>, IOrderRespository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}