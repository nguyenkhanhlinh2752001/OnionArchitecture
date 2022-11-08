using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Application.Repositories
{
    public class CartDetailRepository : RepositoryAsync<CartDetail, int>, ICartDetailRepository
    {
        public CartDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}