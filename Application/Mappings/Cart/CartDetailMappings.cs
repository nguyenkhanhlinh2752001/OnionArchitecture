using Application.Features.CartFeatures.Commands.AddEditCartCommand;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.Cart
{
    public class CartDetailMappings : Profile
    {
        public CartDetailMappings()
        {
            CreateMap<AddEditCartCommand, CartDetail>();
        }
    }
}