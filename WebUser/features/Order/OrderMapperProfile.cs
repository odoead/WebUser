using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.Order.DTO;

namespace WebUser.features.Order
{
    public class OrderMapperProfile : Profile
    {
        public OrderMapperProfile()
        {
            CreateMap<E.Order,OrderDTO>().ReverseMap();
            CreateMap<E.Order, UpdateOrderDTO>().ReverseMap();
        }
    }
}
