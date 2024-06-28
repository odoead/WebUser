using AutoMapper;
using WebUser.features.discount.DTO;
using WebUser.features.Discount.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.discount
{
    public class DiscountMapperProfile : Profile
    {
        public DiscountMapperProfile()
        {
            CreateMap<E.Discount, DiscountDTO>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => DateTime.UtcNow >= src.ActiveFrom && DateTime.UtcNow <= src.ActiveTo))
                .ReverseMap();
            CreateMap<E.Discount, DiscountMinDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => DateTime.UtcNow >= src.ActiveFrom && DateTime.UtcNow <= src.ActiveTo))
                .ReverseMap();
            CreateMap<E.Discount, DiscountUpdateDTO>().ReverseMap();
        }
    }
}
