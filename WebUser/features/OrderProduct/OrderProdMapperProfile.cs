using AutoMapper;
using WebUser.features.OrderProduct.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.OrderProduct
{
    public class OrderProductMapperProfile : Profile
    {
        public OrderProductMapperProfile()
        {
            CreateMap<E.OrderProduct, OrderProductDTO>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.TotalFinalPrice, opt => opt.MapFrom(src => src.Amount * src.FinalPrice))
                .ReverseMap();
            CreateMap<E.OrderProduct, UpdateOrderProdDTO>().ReverseMap();
        }
    }
}
