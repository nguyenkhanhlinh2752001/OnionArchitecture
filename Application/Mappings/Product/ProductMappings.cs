using Application.Dtos.Products;
using Application.Features.ProductFeatures.Commands.AddEditProduct;
using AutoMapper;

namespace Application.Mappings.Product
{
    public class ProductMappings : Profile
    {
        public ProductMappings()
        {
            CreateMap<AddEditProductCommand, Domain.Entities.Product>();
            CreateMap<ProductDetailDto, Domain.Entities.ProductDetail>();
        }
    }
}