using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Persistence.Services;

namespace Application.Features.CartFeatures.Commands.AddEditCartCommand
{
    public class AddEditCartCommand : IRequest<Response<AddEditCartCommand>>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        internal class AddEditCartCommandHandler : IRequestHandler<AddEditCartCommand, Response<AddEditCartCommand>>
        {
            private readonly ICartDetailRepository _cartDetailRepository;
            private readonly ICartRepository _cartRepository;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;

            public AddEditCartCommandHandler(ICartDetailRepository cartDetailRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ICartRepository cartRepository)
            {
                _cartDetailRepository = cartDetailRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _currentUserService = currentUserService;
                _cartRepository = cartRepository;
            }

            public async Task<Response<AddEditCartCommand>> Handle(AddEditCartCommand request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var cart = await _cartRepository.FindAsync(x => x.UserId == userId);
                if (cart == null) throw new ApiException("Cart not found");
                var cartId = cart.Id;

                var cartDetail = await _cartDetailRepository.FindAsync(x => x.CartId == cartId && x.ProductId == request.ProductId);

                if (cartDetail == null)
                {
                    var addCartdetail = _mapper.Map<CartDetail>(request);
                    addCartdetail.CartId = cartId;
                    await _cartDetailRepository.AddAsync(addCartdetail);
                    await _unitOfWork.Commit(cancellationToken);
                }
                else
                {
                    _mapper.Map(request, cartDetail);
                    await _cartDetailRepository.UpdateAsync(cartDetail);
                    await _unitOfWork.Commit(cancellationToken);
                }
                return new Response<AddEditCartCommand>(request);
            }
        }
    }
}