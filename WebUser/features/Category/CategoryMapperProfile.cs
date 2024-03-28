using AutoMapper;
using Entities=WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.features.AttributeValue.DTO;

namespace WebUser.features.Cart
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<Entities.Category, AttributeValueDTO>().ReverseMap();
            CreateMap<Entities.Category, AttributeValueUpdateDTO>().ReverseMap();
        }
    }
}
