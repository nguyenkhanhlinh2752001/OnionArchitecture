using Application.Dtos.Orders;
using AutoMapper;

namespace Application.Mappings.Order
{
    public class OrderMappings : Profile
    {
        public OrderMappings()
        {
            CreateMap<OrderDto, Domain.Entities.Order>();
            CreateMap<OrderDetailDto, Domain.Entities.OrderDetail>();
        }
    }
}