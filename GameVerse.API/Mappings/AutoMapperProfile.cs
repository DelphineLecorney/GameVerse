using AutoMapper;
using GameVerse.SHARED.DTOs.Auth;
using GameVerse.SHARED.DTOs.Games;
using GameVerse.SHARED.DTOs.UserGame;
using GameVerse.SHARED.DTOs.Users;
using GameVerse.API.Models;


namespace GameVerse.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserDto>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<Game, GameDto>();
            CreateMap<CreateGameDto, Game>();

            CreateMap<UserGame, UserGameDto>();
            CreateMap<AddUserGameDto, UserGame>();

        }
    }
}
