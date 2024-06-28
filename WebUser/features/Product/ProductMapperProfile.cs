using AutoMapper;
using WebUser.features.Product.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<E.Product, ProductDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues))
                .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => src.Discounts))
                .ForMember(dest => dest.Coupons, opt => opt.MapFrom(src => src.Coupons))
                .ForMember(dest => dest.Promotions, opt => opt.MapFrom(src => src.Promotions))
                .ReverseMap();
            CreateMap<E.Product, ProductMinDTO>().ReverseMap();
            CreateMap<E.Product, ProductPageDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues))
                .ForMember(dest => dest.Promotions, opt => opt.MapFrom(src => src.Promotions))
                .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => src.Discounts))
                .ReverseMap();
            CreateMap<E.Product, ProductThumbnailDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => src.Discounts))
                .ReverseMap();
        }
    }
}
