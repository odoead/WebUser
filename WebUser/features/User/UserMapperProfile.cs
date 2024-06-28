namespace WebUser.features.User
{
    using AutoMapper;
    using WebUser.features.User.DTO;
    using E = WebUser.Domain.entities;

    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<E.User, UserDTO>()
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders))
                .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points))
                .ReverseMap();
        }
    }
}
