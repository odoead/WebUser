using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.Coupon.DTO;

namespace WebUser.features.Coupon
{
    public class DiscountMapperProfile : Profile
    {
        public DiscountMapperProfile()
        {
            CreateMap<E.Coupon,CouponDTO>().ReverseMap();
            CreateMap<E.Coupon,UpdateCouponDTO>().ReverseMap();
        }
    }
}
