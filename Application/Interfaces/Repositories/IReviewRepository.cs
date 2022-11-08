using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IReviewRepository : IRepositoryAsync<Review, int>
    {
    }
}