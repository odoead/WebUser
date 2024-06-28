using AutoMapper;
using WebUser.features.Coupon.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon
{
    public class DiscountMapperProfile : Profile
    {
        public DiscountMapperProfile()
        {
            CreateMap<E.Coupon, CouponDTO>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(
                    dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActivated && DateTime.UtcNow >= src.ActiveFrom && DateTime.UtcNow <= src.ActiveTo)
                )
                .ReverseMap();
            CreateMap<E.Coupon, CouponMinDTO>()
                .ForMember(
                    dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActivated && DateTime.UtcNow >= src.ActiveFrom && DateTime.UtcNow <= src.ActiveTo)
                )
                .ReverseMap();
            CreateMap<E.Coupon, UpdateCouponDTO>().ReverseMap();
        }
    }
}
