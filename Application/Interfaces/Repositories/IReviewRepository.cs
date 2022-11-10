using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IReviewRepository : RepositoryAsync<Review, int>
    {
    }
}