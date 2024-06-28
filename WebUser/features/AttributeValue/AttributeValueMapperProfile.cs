using AutoMapper;
using WebUser.features.AttributeValue.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.AttributeValue
{
    public class AttributeValueMapperProfile : Profile
    {
        public AttributeValueMapperProfile()
        {
            CreateMap<E.AttributeValue, AttributeValueDTO>().ReverseMap();
            CreateMap<List<E.AttributeValue>, AttributeNameValueDTO>()
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.AttributeName, opt => opt.MapFrom(src => src.FirstOrDefault().AttributeName))
                .ReverseMap();
            CreateMap<E.AttributeValue, AttributeValueUpdateDTO>().ReverseMap();
        }
    }
}
