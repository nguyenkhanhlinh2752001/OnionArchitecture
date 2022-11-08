using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.ProductFeatures.Commands.AddEditProduct
{
    public class AddEditProductCommand : IRequest<Response<AddEditProductCommand>>
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        internal class CreateProductCommandHandler : IRequestHandler<AddEditProductCommand, Response<AddEditProductCommand>>
        {
            private readonly IProductRepsitory _productRepsitory;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;

            public CreateProductCommandHandler(IProductRepsitory productRepsitory, IUnitOfWork<int> unitOfWork, IMapper mapper)
            {
                _productRepsitory = productRepsitory;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Response<AddEditProductCommand>> Handle(AddEditProductCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == 0)
                {
                    var addProduct = _mapper.Map<Product>(request);
                    await _productRepsitory.AddAsync(addProduct);
                    await _unitOfWork.Commit(cancellationToken);
                    request.Id = addProduct.Id;
                }
                else
                {
                    var updateProduct = await _productRepsitory.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
                    if (updateProduct == null) throw new ApiException("Product nor found");
                    _mapper.Map(request, updateProduct);
                    await _productRepsitory.UpdateAsync(updateProduct);
                    await _unitOfWork.Commit(cancellationToken);
                    request.Id = updateProduct.Id;
                }
                return new Response<AddEditProductCommand>(request);
            }
        }
    }
}