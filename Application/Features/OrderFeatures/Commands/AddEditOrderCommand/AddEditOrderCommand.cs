using Application.Dtos.Orders;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Persistence.Services;

namespace Application.Features.OrderFeatures.Commands.AddEditOrderCommand
{
    public class AddEditOrderCommand : OrderDto, IRequest<Response<AddEditOrderCommand>>
    {
        public List<OrderDetailDto>? OrderDetails { get; set; }

        internal class CreateOrderCommandHanler : IRequestHandler<AddEditOrderCommand, Response<AddEditOrderCommand>>
        {
            private readonly IOrderRespository _orderRespository;
            private readonly IOrderDetailRepository _orderDetailRepository;
            private readonly IProductRepository _productRepsitory;
            private readonly ICurrentUserService _currentUserService;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;

            public CreateOrderCommandHanler(IOrderRespository orderRespository, IUnitOfWork<int> unitOfWork, IMapper mapper, IOrderDetailRepository orderDetailRepository, ICurrentUserService currentUserService, IProductRepository productRepsitory)
            {
                _orderRespository = orderRespository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _orderDetailRepository = orderDetailRepository;
                _currentUserService = currentUserService;
                _productRepsitory = productRepsitory;
            }

            public async Task<Response<AddEditOrderCommand>> Handle(AddEditOrderCommand request, CancellationToken cancellationToken)
            {
                var orderId = 0;
                if (request.Id == 0)
                {
                    var addOrder = _mapper.Map<Order>(request);
                    addOrder.UserId = _currentUserService.Id;
                    await _orderRespository.AddAsync(addOrder);
                    await _unitOfWork.Commit(cancellationToken);
                    orderId = addOrder.Id;
                }
                else
                {
                    var updateOrder = await _orderRespository.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
                    if (updateOrder == null) throw new ApiException("Order not found");
                    _mapper.Map(request, updateOrder);
                    await _orderRespository.UpdateAsync(updateOrder);
                    await _unitOfWork.Commit(cancellationToken);
                    orderId = updateOrder.Id;
                }

                if (request.OrderDetails != null)
                {
                    foreach (var item in request.OrderDetails)
                    {
                        var orderDetail = await _orderDetailRepository.FindAsync(x => x.ProductId == item.ProductId && x.OrderId == request.Id && !x.IsDeleted);
                        if (orderDetail == null)
                        {
                            var addOrderdetail = _mapper.Map<OrderDetail>(item);
                            addOrderdetail.OrderId = orderId;
                            await _orderDetailRepository.AddAsync(addOrderdetail);
                            var product = await _productRepsitory.FindAsync(x => x.Id == addOrderdetail.ProductId);
                            if (product == null) throw new ApiException("Product not found");
                            _mapper.Map(product, product);
                            await _productRepsitory.UpdateAsync(product);
                            await _unitOfWork.Commit(cancellationToken);
                        }
                        else
                        {
                            var oldQuantity = orderDetail.Quantity;
                            _mapper.Map(item, orderDetail);
                            var product = await _productRepsitory.FindAsync(x => x.Id == orderDetail.ProductId);
                            await _orderDetailRepository.UpdateAsync(orderDetail);
                            if (product == null) throw new ApiException("Product not found");
                            _mapper.Map(product, product);
                            await _productRepsitory.UpdateAsync(product);
                            await _unitOfWork.Commit(cancellationToken);
                        }
                    }
                }
                return new Response<AddEditOrderCommand>(request);
            }
        }
    }
}