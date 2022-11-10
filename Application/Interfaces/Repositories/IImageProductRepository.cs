using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IImageProductRepository : RepositoryAsync<ImageProduct, int>
    {
    }
}