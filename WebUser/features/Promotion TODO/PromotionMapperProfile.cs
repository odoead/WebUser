using AutoMapper;
using WebUser.features.Promotion.DTO;
using WebUser.features.Promotion_TODO.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion
{
    public class PromotionMapperProfile : Profile
    {
        public PromotionMapperProfile()
        {
            CreateMap<E.Promotion, PromotionDTO>()
                .ForMember(dest => dest.PromoProducts, opt => opt.MapFrom(src => src.PromoProducts))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => DateTime.UtcNow >= src.ActiveFrom && DateTime.UtcNow <= src.ActiveTo))
                .ReverseMap();
            CreateMap<E.Promotion, PromotionMinDTO>().ReverseMap();
            CreateMap<E.Promotion, PromotionPageDTO>()
                .ForMember(dest => dest.HoursLeft, opt => opt.MapFrom(src => src.ActiveTo.Hour - DateTime.UtcNow.Hour))
                .ForMember(dest => dest.DaysLeft, opt => opt.MapFrom(src => src.ActiveTo.Day - DateTime.UtcNow.Day))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => DateTime.UtcNow >= src.ActiveFrom && DateTime.UtcNow <= src.ActiveTo))
                .ReverseMap();
            CreateMap<E.Promotion, PromotionProductPageDTO>().ReverseMap();
            CreateMap<E.Promotion, PromotionThumbnailDTO>()
                .ForMember(dest => dest.HoursLeft, opt => opt.MapFrom(src => src.ActiveTo.Hour - DateTime.UtcNow.Hour))
                .ForMember(dest => dest.DaysLeft, opt => opt.MapFrom(src => src.ActiveTo.Day - DateTime.UtcNow.Day))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => DateTime.UtcNow >= src.ActiveFrom && DateTime.UtcNow <= src.ActiveTo))
                .ReverseMap();
        }
    }
}
