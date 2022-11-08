using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Application.Repositories
{
    public class ReviewRepository : RepositoryAsync<Review, int>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}