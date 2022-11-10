using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository : RepositoryAsync<Category, int>
    {
    }
}