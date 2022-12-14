using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.ProductFeatures.Commands.DeleteProductById
{
    public class DeleteProductByIdCommand : IRequest<Response<DeleteProductByIdCommand>>
    {
        public int Id { get; set; }

        internal class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<DeleteProductByIdCommand>>
        {
            private readonly IProductRepository _productRepsitory;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteProductByIdCommandHandler(IProductRepository productRepsitory, IUnitOfWork<int> unitOfWork, IProductDetailRepository productDetailRepository)
            {
                _productRepsitory = productRepsitory;
                _unitOfWork = unitOfWork;
                _productDetailRepository = productDetailRepository;
            }

            public async Task<Response<DeleteProductByIdCommand>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
            {
                var product = await _productRepsitory.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
                if (product == null) throw new ApiException("Product not found");
                product.IsDeleted = true;
                await _productRepsitory.UpdateAsync(product);
                await _unitOfWork.Commit(cancellationToken);

                var listProductdetails = await _productDetailRepository.GetByCondition(x => x.ProductId == product.Id);
                foreach (var item in listProductdetails)
                {
                    item.IsDeleted = true;
                    await _productDetailRepository.UpdateAsync(item);
                    await _unitOfWork.Commit(cancellationToken);
                }
                return new Response<DeleteProductByIdCommand>(request);
            }
        }
    }
}