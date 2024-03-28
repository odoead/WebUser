using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.discount.DTO;
using WebUser.features.Image.DTO;

namespace WebUser.features.Image
{
    public class ImageMapperProfile : Profile
    {
        public ImageMapperProfile()
        {
            CreateMap<E.Image,ImageDTO>().ReverseMap();
        }
    }
}
