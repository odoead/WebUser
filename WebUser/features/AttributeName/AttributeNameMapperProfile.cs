using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;

namespace WebUser.features.AttributeName
{
    public class AttributeNameMapperProfile:Profile
    {
        public AttributeNameMapperProfile()
        {
            CreateMap<E.AttributeName, AttributeNameDTO>().ReverseMap();
            CreateMap<E.AttributeName, AttributeNameUpdateDTO>().ReverseMap();
        }
    }
}
