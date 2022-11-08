using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Application.Repositories
{
    public class ProductRepository : RepositoryAsync<Product, int>, IProductRepsitory
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}