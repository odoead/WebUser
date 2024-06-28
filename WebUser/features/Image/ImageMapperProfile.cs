using AutoMapper;
using WebUser.features.Image.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Image
{
    public class ImageMapperProfile : Profile
    {
        public ImageMapperProfile()
        {
            CreateMap<E.Image, ImageDTO>().ReverseMap();
        }
    }
}
