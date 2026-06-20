
using AutoMapper;
using GameVerse.API.DTOs.UserGame;
using GameVerse.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameVerse.API.Controllers
{   
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserGamesController : ControllerBase
    {
        private readonly IUserGameService _userGameService;
        private readonly IMapper _mapper;

        public UserGamesController(IUserGameService userGameService, IMapper mapper)
        {
            _userGameService = userGameService;
            _mapper = mapper;
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> AddUserGame(AddUserGameDto userGameDto)
        {
            var userGame = await _userGameService.AddAsync(userGameDto);
            return Ok(_mapper.Map<UserGameDto>(userGame));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserGames(int userId)
        {
            var list = await _userGameService.GetByUserAsync(userId);
            return Ok(_mapper.Map<IEnumerable<UserGameDto>>(list));
        }

        [HttpGet("user/{userId}/favorites")]
        public async Task<IActionResult> GetFavorites(int userId)
        {
            var list = await _userGameService.GetFavoritesAsync(userId);
            return Ok(_mapper.Map<IEnumerable<UserGameDto>>(list));
        }

        [HttpPut("{userId}/{gameId}")]
        public async Task<IActionResult> UpdateRelation(int userId, int gameId, UpdateUserGameDto updateUserGameDto)
        {
            var updated = await _userGameService.UpdateAsync(userId, gameId, updateUserGameDto);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserGameDto>(updated));
        }

        [HttpDelete("{userId}/{gameId}")]
        public async Task<IActionResult> RemoveUserGame(int userId, int gameId)
        {
            var deleted = await _userGameService.RemoveAsync(userId,gameId);
            if(!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
