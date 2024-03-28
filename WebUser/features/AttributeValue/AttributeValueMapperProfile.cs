using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.AttributeValue.DTO;

namespace WebUser.features.AttributeValue
{
    public class AttributeValueMapperProfile : Profile
    {
        public AttributeValueMapperProfile()
        {
            CreateMap<E.AttributeValue,AttributeValueDTO>().ReverseMap();
            CreateMap<E.AttributeValue, AttributeValueUpdateDTO>().ReverseMap();
        }
    }
}
