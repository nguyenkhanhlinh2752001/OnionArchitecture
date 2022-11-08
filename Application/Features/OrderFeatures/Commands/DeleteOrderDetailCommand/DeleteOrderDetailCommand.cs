using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.OrderFeatures.Commands.DeleteOrderDetailCommand
{
    public class DeleteOrderDetailCommand : IRequest<Response<DeleteOrderDetailCommand>>
    {
        public int Id { get; set; }

        internal class DeleteOrderDetailCommandHandler : IRequestHandler<DeleteOrderDetailCommand, Response<DeleteOrderDetailCommand>>
        {
            private readonly IOrderDetailRepository _orderDetailRepository;
            private readonly IProductRepsitory _productRepsitory;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;

            public DeleteOrderDetailCommandHandler(IOrderDetailRepository orderDetailRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IProductRepsitory productRepsitory)
            {
                _orderDetailRepository = orderDetailRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _productRepsitory = productRepsitory;
            }

            public async Task<Response<DeleteOrderDetailCommand>> Handle(DeleteOrderDetailCommand request, CancellationToken cancellationToken)
            {
                var orderdetail = await _orderDetailRepository.FindAsync(x => x.Id == request.Id);
                if (orderdetail == null) throw new ApiException("Order detail not found");
                orderdetail.IsDeleted = true;
                await _orderDetailRepository.UpdateAsync(orderdetail);
                await _unitOfWork.Commit(cancellationToken);

                var product = await _productRepsitory.FindAsync(x => x.Id == orderdetail.ProductId && !x.IsDeleted);
                if (product == null) throw new ApiException("Product not found");
                product.Quantity = product.Quantity + orderdetail.Quantity;
                await _productRepsitory.UpdateAsync(product);
                await _unitOfWork.Commit(cancellationToken);

                return new Response<DeleteOrderDetailCommand>(request);
            }
        }
    }
}