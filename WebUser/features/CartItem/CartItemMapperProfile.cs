using AutoMapper;
using WebUser.features.CartItem.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class CartItemMapperProfile : Profile
    {
        public CartItemMapperProfile()
        {
            CreateMap<E.CartItem, CartItemDTO>().ForMember(dest => dest.ProductMin, opt => opt.MapFrom(src => src.Product)).ReverseMap();
        }
    }
}
