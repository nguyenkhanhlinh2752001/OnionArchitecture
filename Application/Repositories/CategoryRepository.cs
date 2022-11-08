using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Application.Repositories
{
    public class CategoryRepository : RepositoryAsync<Category, int>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}