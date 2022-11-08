using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepositoryAsync<Category, int>
    {
    }
}