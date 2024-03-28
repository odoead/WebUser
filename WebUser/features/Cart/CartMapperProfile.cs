using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.Cart.DTO;

namespace WebUser.features.Cart
{
    public class CartMapperProfile : Profile
    {
        public CartMapperProfile()
        {
            CreateMap<CartDTO,E.Cart>().ReverseMap();
            CreateMap<CartDTO, E.Cart>().ReverseMap();
        }
    }
}
