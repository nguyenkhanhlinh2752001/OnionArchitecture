using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Application.Repositories
{
    public class ImageReviewRepsitory : RepositoryAsync<ImageReview, int>, IImageReviewRepository
    {
        public ImageReviewRepsitory(ApplicationDbContext context) : base(context)
        {
        }
    }
}