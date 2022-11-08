using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Persistence.Services;

namespace Application.Features.ReviewFeatures.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest<Response<DeleteReviewCommand>>
    {
        public int Id { get; set; }

        internal class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Response<DeleteReviewCommand>>
        {
            private readonly IReviewRepository _reviewRepository;
            private readonly ICurrentUserService _currentUserService;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteReviewCommandHandler(IReviewRepository reviewRepository, ICurrentUserService currentUserService, IUnitOfWork<int> unitOfWork)
            {
                _reviewRepository = reviewRepository;
                _currentUserService = currentUserService;
                _unitOfWork = unitOfWork;
            }

            public async Task<Response<DeleteReviewCommand>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var review = await _reviewRepository.FindAsync(x => x.Id == request.Id && x.UserId == userId);
                if (review == null) throw new ApiException("Review not found");
                review.IsDeleted = true;
                await _reviewRepository.UpdateAsync(review);
                await _unitOfWork.Commit(cancellationToken);
                return new Response<DeleteReviewCommand>(request);
            }
        }
    }
}