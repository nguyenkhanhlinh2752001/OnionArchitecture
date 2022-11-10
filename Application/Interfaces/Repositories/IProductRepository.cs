using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository : RepositoryAsync<Product, int>
    {
    }
}