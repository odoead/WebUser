using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.OrderProduct.DTO;

namespace WebUser.features.OrderProduct
{
    public class OrderProductMapperProfile : Profile
    {
        public OrderProductMapperProfile()
        {
            CreateMap<E.OrderProduct,OrderProductDTO>().ReverseMap();
            CreateMap<E.OrderProduct, OrderProductDTO>().ReverseMap();

        }
    }
}
