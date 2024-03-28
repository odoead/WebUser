using AutoMapper;
using E=WebUser.Domain.entities;
using WebUser.features.Promotion.DTO;

namespace WebUser.features.Promotion
{
    public class PromotionMapperProfile : Profile
    {
        public PromotionMapperProfile()
        {
            CreateMap<E.Promotion,PromotionDTO>().ReverseMap();
            CreateMap<E.Promotion, PromotionDTO>().ReverseMap();

        }
    }
}
