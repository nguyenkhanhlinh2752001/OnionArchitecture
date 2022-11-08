using Application.Features.CartFeatures.Queries.GetCartByUserIdQuery;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Persistence.Services;

namespace Application.Features.CartFeatures.Queries.GetCartByUserIdCommand
{
    public class GetCartByUserIdQuery : IRequest<Response<IEnumerable<GetCartByUserIdViewModel>>>
    {
        internal class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, Response<IEnumerable<GetCartByUserIdViewModel>>>
        {
            private readonly ICurrentUserService _currentUserService;
            private readonly ICartDetailRepository _cartDetailRepository;
            private readonly IProductRepsitory _productRepsitory;
            private readonly ICartRepository _cartRepository;

            public GetCartByUserIdQueryHandler(ICurrentUserService currentUserService, ICartDetailRepository cartDetailRepository, ICartRepository cartRepository, IProductRepsitory productRepsitory)
            {
                _currentUserService = currentUserService;
                _cartDetailRepository = cartDetailRepository;
                _cartRepository = cartRepository;
                _productRepsitory = productRepsitory;
            }

            public async Task<Response<IEnumerable<GetCartByUserIdViewModel>>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var cart = await _cartRepository.FindAsync(x => x.UserId == userId);
                var list = (from c in _cartRepository.Entities
                            join cd in _cartDetailRepository.Entities
                            on c.Id equals cd.CartId
                            join p in _productRepsitory.Entities
                            on cd.ProductId equals p.Id
                            where !cd.IsDeleted
                            select new GetCartByUserIdViewModel
                            {
                                CartDetailId = cd.Id,
                                ProductName = p.Name,
                                Quantity = cd.Quantity,
                                Price = p.Price,
                                ToTal = p.Price * cd.Quantity
                            }).ToList();
                return new Response<IEnumerable<GetCartByUserIdViewModel>>(list);
            }
        }
    }
}