using AutoMapper;
using WebUser.features.AttributeName.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.AttributeName
{
    public class AttributeNameMapperProfile : Profile
    {
        public AttributeNameMapperProfile()
        {
            CreateMap<E.AttributeName, AttributeNameDTO>()
                .ReverseMap()
                .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues));
            CreateMap<E.AttributeName, AttributeNameUpdateDTO>().ReverseMap();
            CreateMap<E.AttributeName, AttributeNameMinDTO>().ReverseMap();
        }
    }
}
