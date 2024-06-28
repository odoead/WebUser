using AutoMapper;
using WebUser.features.Cart.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class CartMapperProfile : Profile
    {
        public CartMapperProfile()
        {
            CreateMap<CartDTO, E.Cart>().ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)).ReverseMap();
            CreateMap<CartDTO, E.Cart>().ReverseMap();
        }
    }
}
