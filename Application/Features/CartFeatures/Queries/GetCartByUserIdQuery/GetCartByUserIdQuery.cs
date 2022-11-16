using Application.Exceptions;
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
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IProductRepository _productRepository;
            private readonly ICartRepository _cartRepository;

            public GetCartByUserIdQueryHandler(ICurrentUserService currentUserService, ICartDetailRepository cartDetailRepository, ICartRepository cartRepository, IProductDetailRepository productDetailRepository, IProductRepository productRepository)
            {
                _currentUserService = currentUserService;
                _cartDetailRepository = cartDetailRepository;
                _cartRepository = cartRepository;
                _productDetailRepository = productDetailRepository;
                _productRepository = productRepository;
            }

            public async Task<Response<IEnumerable<GetCartByUserIdViewModel>>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var cart = await _cartRepository.FindAsync(x => x.UserId == userId);
                if (cart == null) throw new ApiException("Cart not found");
                var list = (from c in _cartRepository.Entities
                            join cd in _cartDetailRepository.Entities
                            on c.Id equals cd.CartId into leftjoinCartdetail
                            from cartdetail in leftjoinCartdetail.DefaultIfEmpty()
                            join pd in _productDetailRepository.Entities
                            on cartdetail.ProductDetailId equals pd.Id
                            join p in _productRepository.Entities
                            on pd.ProductId equals p.Id
                            where !cartdetail.IsDeleted && c.Id == cart.Id
                            select new GetCartByUserIdViewModel
                            {
                                CartDetailId = cartdetail.Id,
                                ProductName = p.Name,
                                Quantity = cartdetail.Quantity,
                                Price = pd.Price,
                                Color = pd.Color,
                                ToTal = pd.Price * cartdetail.Quantity
                            }).ToList();
                return new Response<IEnumerable<GetCartByUserIdViewModel>>(list);
            }
        }
    }
}