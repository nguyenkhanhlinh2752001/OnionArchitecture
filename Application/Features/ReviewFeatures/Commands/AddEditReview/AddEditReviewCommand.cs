using Application.Dtos.Reviews;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Persistence.Services;

namespace Application.Features.ReviewFeatures.Commands.AddEditReview
{
    public class AddEditReviewCommand : ReviewDto, IRequest<Response<AddEditReviewCommand>>
    {
        //public List<ImageReviewDto>? Images { get; set; }

        //public string UserId { get; set; }
        //public int ProductDetailId { get; set; }
        //public string Title { get; set; }
        //public string Content { get; set; }
        //public int Rate { get; set; }

        public int Id { get; set; }
        public int ProductDetailId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rate { get; set; }
        public IEnumerable<ImageReviewDto>? ImageReviews { get; set; }

        internal class AddEditReviewCommandHandler : IRequestHandler<AddEditReviewCommand, Response<AddEditReviewCommand>>
        {
            private readonly IReviewRepository _reviewRepository;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly ICurrentUserService _currentUserService;
            private readonly IImageReviewRepository _imageReviewRepository;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;

            public AddEditReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IProductDetailRepository productDetailRepository, ICurrentUserService currentUserService, IImageReviewRepository imageReviewRepository)
            {
                _reviewRepository = reviewRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _productDetailRepository = productDetailRepository;
                _currentUserService = currentUserService;
                _imageReviewRepository = imageReviewRepository;
            }

            public async Task<Response<AddEditReviewCommand>> Handle(AddEditReviewCommand request, CancellationToken cancellationToken)
            {
                var reviewId = 0;
                var productDetail = await _productDetailRepository.FindAsync(x => x.Id == request.ProductDetailId);
                if (productDetail == null) throw new ApiException("Product detail not found");
                if (request.Id == 0)
                {
                    var addReview = _mapper.Map<Review>(request);
                    addReview.UserId = _currentUserService.Id;
                    await _reviewRepository.AddAsync(addReview);
                    await _unitOfWork.Commit(cancellationToken);
                    reviewId = addReview.Id;
                }
                else
                {
                    var updateReview = await _reviewRepository.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
                    if (updateReview == null) throw new ApiException("Review not found");
                    _mapper.Map(request, updateReview);
                    await _reviewRepository.UpdateAsync(updateReview);
                    await _unitOfWork.Commit(cancellationToken);
                    reviewId = updateReview.Id;

                    var listImages = await _imageReviewRepository.GetByCondition(x => x.ReviewId == updateReview.Id);
                    foreach (var item in listImages)
                    {
                        item.IsDeleted = true;
                        await _imageReviewRepository.UpdateAsync(item);
                        await _unitOfWork.Commit(cancellationToken);
                    }
                }

                if (request.ImageReviews != null)
                {
                    foreach (var item in request.ImageReviews)
                    {
                        var addImageReview = _mapper.Map<ImageReview>(item);
                        addImageReview.ReviewId = reviewId;
                        await _imageReviewRepository.AddAsync(addImageReview);
                        await _unitOfWork.Commit(cancellationToken);
                    }
                }
                return new Response<AddEditReviewCommand>(request);
            }
        }
    }
}