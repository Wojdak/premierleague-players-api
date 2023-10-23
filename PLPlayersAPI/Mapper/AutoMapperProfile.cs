using AutoMapper;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Models;

namespace PLPlayersAPI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Player, PlayerDTO>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue ? src.DateOfBirth.Value.ToString("yyyy-MM-dd") : null));
            CreateMap<Club, ClubDTO>();
            CreateMap<Nationality, NationalityDTO>();
            CreateMap<Position, PositionDTO>();
            CreateMap<UserDTO, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
               .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Standard"));
        }
    }
}
