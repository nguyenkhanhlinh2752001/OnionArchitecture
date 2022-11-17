using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.ProductFeatures.Commands.DeleteProdutcDetailById
{
    public class DeleteProductDetailByIdCommand : IRequest<Response<DeleteProductDetailByIdCommand>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductDetailByIdCommandHandler : IRequestHandler<DeleteProductDetailByIdCommand, Response<DeleteProductDetailByIdCommand>>
    {
        private readonly IProductDetailRepository _productDetailRepository;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteProductDetailByIdCommandHandler(IProductDetailRepository productDetailRepository, IUnitOfWork<int> unitOfWork)
        {
            _productDetailRepository = productDetailRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<DeleteProductDetailByIdCommand>> Handle(DeleteProductDetailByIdCommand request, CancellationToken cancellationToken)
        {
            var productDetail = await _productDetailRepository.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
            if (productDetail == null) throw new ApiException("Product detail not found");
            productDetail.IsDeleted = true;
            await _productDetailRepository.UpdateAsync(productDetail);
            await _unitOfWork.Commit(cancellationToken);
            return new Response<DeleteProductDetailByIdCommand>(request);
        }
    }
}