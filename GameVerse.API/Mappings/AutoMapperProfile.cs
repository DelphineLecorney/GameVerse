using AutoMapper;
using GameVerse.API.DTOs.Auth;
using GameVerse.API.DTOs.Games;
using GameVerse.API.DTOs.UserGame;
using GameVerse.API.DTOs.Users;
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
