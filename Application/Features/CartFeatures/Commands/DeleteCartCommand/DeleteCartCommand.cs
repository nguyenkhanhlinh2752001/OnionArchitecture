using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Persistence.Services;

namespace Application.Features.CartFeatures.Commands.DeleteCartCommand
{
    public class DeleteCartCommand : IRequest<Response<DeleteCartCommand>>
    {
        public int Id { get; set; }

        internal class DeleteCartCommandHanlder : IRequestHandler<DeleteCartCommand, Response<DeleteCartCommand>>
        {
            private readonly ICartDetailRepository _cartDetailRepository;
            private readonly ICartRepository _cartRepository;
            private readonly ICurrentUserService _currentUserService;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteCartCommandHanlder(ICartDetailRepository cartDetailRepository, IUnitOfWork<int> unitOfWork, ICartRepository cartRepository, ICurrentUserService currentUserService)
            {
                _cartDetailRepository = cartDetailRepository;
                _unitOfWork = unitOfWork;
                _cartRepository = cartRepository;
                _currentUserService = currentUserService;
            }

            public async Task<Response<DeleteCartCommand>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var cart = await _cartRepository.FindAsync(x => x.UserId == userId);
                if (cart == null) throw new ApiException("Cart not found");
                var cartDetail = await _cartDetailRepository.FindAsync(x => x.CartId == cart.Id && x.Id == request.Id);
                if (cartDetail == null) throw new ApiException("Cart detail not found");
                cartDetail.IsDeleted = true;
                await _cartDetailRepository.UpdateAsync(cartDetail);
                await _unitOfWork.Commit(cancellationToken);
                return new Response<DeleteCartCommand>(request);
            }
        }
    }
}