using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProductDetailRepository : RepositoryAsync<ProductDetail, int>
    {
    }
}