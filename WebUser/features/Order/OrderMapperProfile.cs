using AutoMapper;
using WebUser.features.Order.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Order
{
    public class OrderMapperProfile : Profile
    {
        public OrderMapperProfile()
        {
            CreateMap<E.Order, OrderDTO>()
                .ForMember(dest => dest.ActivatedCoupons, opt => opt.MapFrom(src => src.ActivatedCoupons))
                .ForMember(dest => dest.ActivatedPoints, opt => opt.MapFrom(src => src.Points))
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProduct))
                .ReverseMap();
            CreateMap<E.Order, OrderUserDTO>().ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProduct)).ReverseMap();
            CreateMap<E.Order, UpdateOrderDTO>().ReverseMap();
        }
    }
}
