using AutoMapper;
using WebUser.features.Category.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<E.Category, CategoryDTO>()
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.Attributes))
                .ForMember(dest => dest.ParentCategory, opt => opt.MapFrom(src => src.ParentCategory))
                .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories))
                .ReverseMap();
            CreateMap<E.Category, CategoryMinDTO>().ReverseMap();
        }
    }
}
