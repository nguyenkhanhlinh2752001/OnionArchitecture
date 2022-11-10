using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Persistence.Services;

namespace Application.Features.ReviewFeatures.Commands.AddEditReview
{
    public class AddEditReviewCommand : IRequest<Response<AddEditReviewCommand>>
    {
        public int ProductDetailId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int Rate { get; set; }
        public IEnumerable<string>? Images { get; set; }

        internal class AddEditReviewCommandHandler : IRequestHandler<AddEditReviewCommand, Response<AddEditReviewCommand>>
        {
            private readonly IReviewRepository _reviewRepository;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;

            public AddEditReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
            {
                _reviewRepository = reviewRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _currentUserService = currentUserService;
            }

            public async Task<Response<AddEditReviewCommand>> Handle(AddEditReviewCommand request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var review = await _reviewRepository.FindAsync(x => x.UserId == userId );
                if (review == null)
                {
                    var addReview = _mapper.Map<Review>(request);
                    addReview.UserId = userId;
                    await _reviewRepository.AddAsync(addReview);
                    await _unitOfWork.Commit(cancellationToken);
                }
                else
                {
                    _mapper.Map(request, review);
                    await _reviewRepository.UpdateAsync(review);
                    await _unitOfWork.Commit(cancellationToken);
                }
                return new Response<AddEditReviewCommand>(request);
            }
        }
    }
}