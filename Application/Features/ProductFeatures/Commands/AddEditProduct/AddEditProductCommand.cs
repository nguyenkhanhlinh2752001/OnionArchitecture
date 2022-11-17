using Application.Dtos.Products;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductFeatures.Commands.AddEditProduct
{
    public class AddEditProductCommand : IRequest<Response<AddEditProductCommand>>
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<ProductDetailDto>? ProductDetails { get; set; }

        internal class CreateProductCommandHandler : IRequestHandler<AddEditProductCommand, Response<AddEditProductCommand>>
        {
            private readonly IProductRepository _productRepsitory;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;

            public CreateProductCommandHandler(IProductRepository productRepsitory, IUnitOfWork<int> unitOfWork, IMapper mapper, IProductDetailRepository productDetailRepository)
            {
                _productRepsitory = productRepsitory;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _productDetailRepository = productDetailRepository;
            }

            public async Task<Response<AddEditProductCommand>> Handle(AddEditProductCommand request, CancellationToken cancellationToken)
            {
                var productId = 0;
                if (request.Id == 0)
                {
                    var addProduct = _mapper.Map<Product>(request);
                    await _productRepsitory.AddAsync(addProduct);
                    await _unitOfWork.Commit(cancellationToken);
                    productId = addProduct.Id;
                }
                else
                {
                    var updateProduct = await _productRepsitory.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
                    if (updateProduct == null) throw new ApiException("Product not found");
                    _mapper.Map(request, updateProduct);
                    await _productRepsitory.UpdateAsync(updateProduct);
                    await _unitOfWork.Commit(cancellationToken);
                    productId = updateProduct.Id;
                }

                if (request.ProductDetails != null)
                {
                    foreach (var item in request.ProductDetails)
                    {
                        var productDetail = await _productDetailRepository.Entities.FirstOrDefaultAsync(x => x.Color.ToLower().Equals(item.Color!.ToLower()) && x.ProductId == productId);
                        if (productDetail == null)
                        {
                            var addProductdetail = _mapper.Map<ProductDetail>(item);
                            addProductdetail.ProductId = productId;
                            await _productDetailRepository.AddAsync(addProductdetail);
                            await _unitOfWork.Commit(cancellationToken);
                        }
                        else
                        {
                            _mapper.Map(item, productDetail);
                            await _productDetailRepository.UpdateAsync(productDetail);
                            await _unitOfWork.Commit(cancellationToken);
                        }
                    }
                }
                return new Response<AddEditProductCommand>(request);
            }
        }
    }
}