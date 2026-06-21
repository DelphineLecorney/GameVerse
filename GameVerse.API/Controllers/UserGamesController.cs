using AutoMapper;
using FluentValidation;
using GameVerse.API.DTOs.UserGame;
using GameVerse.API.Services;
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
        private readonly IValidator<AddUserGameDto> _addValidator;
        private readonly IValidator<UpdateUserGameDto> _updateValidator;

        public UserGamesController(
            IUserGameService userGameService,
            IMapper mapper,
            IValidator<AddUserGameDto> addValidator,
            IValidator<UpdateUserGameDto> updateValidator)
        {
            _userGameService = userGameService;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        [HttpPost("add")]
        [ProducesResponseType(typeof(UserGameDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> AddUserGame(AddUserGameDto userGameDto)
        {
            var validation = await _addValidator.ValidateAsync(userGameDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToDictionary());

            var userGame = await _userGameService.AddAsync(userGameDto);

            return CreatedAtAction(nameof(GetUserGames), 
                new { userId = userGame.UserId },
                _mapper.Map<UserGameDto>(userGame));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserGames(int userId)
        {
            var list = await _userGameService.GetByUserAsync(userId);
            return Ok(_mapper.Map<IEnumerable<UserGameDto>>(list));
        }

        [HttpGet("user/{userId}/favorites")]
        [ProducesResponseType(typeof(IEnumerable<UserGameDto>), 200)]
        public async Task<IActionResult> GetFavorites(int userId)
        {
            var list = await _userGameService.GetFavoritesAsync(userId);
            return Ok(_mapper.Map<IEnumerable<UserGameDto>>(list));
        }

        [HttpPut("{userId}/{gameId}")]
        public async Task<IActionResult> UpdateRelation(int userId, int gameId, UpdateUserGameDto updateUserGameDto)
        {
            var validation = await _updateValidator.ValidateAsync(updateUserGameDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToDictionary());

            var updated = await _userGameService.UpdateAsync(userId, gameId, updateUserGameDto);
            if (updated == null)
                return NotFound();

            return Ok(_mapper.Map<UserGameDto>(updated));
        }

        [HttpDelete("{userId}/{gameId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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
