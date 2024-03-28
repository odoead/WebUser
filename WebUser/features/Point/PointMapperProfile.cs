using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.Point.DTO;

namespace WebUser.features.Point
{
    public class PromotionMapperProfile : Profile
    {
        public PromotionMapperProfile()
        {
            CreateMap<E.Point, PointDTO>().ReverseMap();

        }
    }
}
