using AutoMapper;
using WebUser.features.Point.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Point
{
    public class PromotionMapperProfile : Profile
    {
        public PromotionMapperProfile()
        {
            CreateMap<E.Point, PointDTO>()
                .ForMember(
                    dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsUsed && DateTime.UtcNow >= src.CreateDate && DateTime.UtcNow <= src.ExpireDate)
                )
                .ReverseMap();
            CreateMap<E.Point, PointMinDTO>()
                .ForMember(
                    dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsUsed && DateTime.UtcNow >= src.CreateDate && DateTime.UtcNow <= src.ExpireDate)
                )
                .ReverseMap();
        }
    }
}
