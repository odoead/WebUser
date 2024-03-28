using AutoMapper;
using WebUser.features.Cart.DTO;
using E=WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class CartItemMapperProfile : Profile
    {
        public CartItemMapperProfile()
        {
            CreateMap<E.CartItem,CartDTO>().ReverseMap();
            CreateMap<E.CartItem,CartUpdateDTO>().ReverseMap();
        }
    }
}
