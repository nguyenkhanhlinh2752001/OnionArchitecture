using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands.DeleteOrderCommand
{
    public class DeleteOrderCommand : IRequest<Response<DeleteOrderCommand>>
    {
        public int Id { get; set; }

        internal class DeleteOrderCommandHanlder : IRequestHandler<DeleteOrderCommand, Response<DeleteOrderCommand>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IOrderRespository _orderRespository;
            private readonly IOrderDetailRepository _orderDetailRepository;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteOrderCommandHanlder(ApplicationDbContext context, IOrderRespository orderRespository, IOrderDetailRepository orderDetailRepository, IUnitOfWork<int> unitOfWork, IProductDetailRepository productDetailRepository)
            {
                _context = context;
                _orderRespository = orderRespository;
                _orderDetailRepository = orderDetailRepository;
                _unitOfWork = unitOfWork;
                _productDetailRepository = productDetailRepository;
            }

            public async Task<Response<DeleteOrderCommand>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var order = await _orderRespository.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
                if (order == null) throw new ApiException("Order not found");
                order.IsDeleted = true;
                await _orderRespository.UpdateAsync(order);
                await _unitOfWork.Commit(cancellationToken);

                var listOrderdetails = await _orderDetailRepository.GetByCondition(x => x.OrderId == order.Id);
                foreach (var item in listOrderdetails)
                {
                    item.IsDeleted = true;
                    await _orderDetailRepository.UpdateAsync(item);
                    await _unitOfWork.Commit(cancellationToken);

                    var product = await _productDetailRepository.FindAsync(x => x.Id == item.ProductDetailId);
                    if (product == null) throw new ApiException("Product detail not found");
                    await _productDetailRepository.UpdateAsync(product);
                    await _unitOfWork.Commit(cancellationToken);
                }

                return new Response<DeleteOrderCommand>(request);
                //var dbContextTransaction = _context.Database.BeginTransaction();
                //try
                //{
                //    var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.Id);
                //    if (order == null) throw new ApiException("Order not found");
                //    var orderDetails = _context.OrderDetails.Where(od => od.OrderId == request.Id).ToList();
                //    foreach (var orderDetail in orderDetails)
                //    {
                //        orderDetail.IsDeleted = true;
                //        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderDetail.ProductId);
                //        if (product == null) throw new ApiException("Product not found");
                //        product.Quantity += orderDetail.Quantity;
                //    }
                //    order.IsDeleted = true;
                //    await _context.SaveChangesAsync();
                //    await dbContextTransaction.CommitAsync();
                //    await dbContextTransaction.DisposeAsync();

                //    return new Response<int>(order.Id);
                //}
                //catch (Exception)
                //{
                //    await dbContextTransaction.RollbackAsync();
                //    await dbContextTransaction.DisposeAsync();
                //    throw;
                //}
            }
        }
    }
}