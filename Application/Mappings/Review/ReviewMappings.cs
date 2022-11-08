using Application.Features.ReviewFeatures.Commands.AddEditReview;
using AutoMapper;

namespace Application.Mappings.Review
{
    public class ReviewMappings : Profile
    {
        public ReviewMappings()
        {
            CreateMap<AddEditReviewCommand, Domain.Entities.Review>();
        }
    }
}