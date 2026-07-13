using AutoMapper;
using FluentValidation;
using GameVerse.API.Extensions;
using GameVerse.API.Services.Interfaces;
using GameVerse.SHARED.DTOs.Games;
using GameVerse.SHARED.DTOs.UserGame;
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
        public async Task<IActionResult> AddUserGame(AddUserGameDto userGameDto)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return BadRequest("User not authenticated.");

            userGameDto.UserId = userId;

            var validation = await _addValidator.ValidateAsync(userGameDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToDictionary());

            var userGame = await _userGameService.AddOrUpdateAsync(userGameDto);

            return Ok(_mapper.Map<UserGameDto>(userGame));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserGames(string userId)
        {
            var list = await _userGameService.GetByUserAsync(userId);
            return Ok(_mapper.Map<IEnumerable<UserGameDto>>(list));
        }


        [HttpPut("{userId}/{gameId}")]
        public async Task<IActionResult> UpdateRelation(string userId, int gameId, UpdateUserGameDto updateUserGameDto)
        {
            var validation = await _updateValidator.ValidateAsync(updateUserGameDto);
            if (!validation.IsValid)
                return BadRequest(validation.ToDictionary());

            var updated = await _userGameService.UpdateAsync(userId, gameId, updateUserGameDto);
            if (updated == null)
                return NotFound();

            return Ok(_mapper.Map<UserGameDto>(updated));
        }

        [HttpDelete("{gameId}")]
        public async Task<IActionResult> RemoveUserGame(int gameId)
        {
            var userId = User.GetUserId();

            if (userId == null)
                return BadRequest("User not authenticated.");

            var deleted = await _userGameService.RemoveAsync(userId, gameId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("wishlist")]
        public async Task<IActionResult> GetWishlist()
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("User not authenticated.");

            var all = await _userGameService.GetByUserAsync(userId);
            var wishlist = all.Where(ug => ug.RelationType == "Wishlist");
            return Ok(_mapper.Map<IEnumerable<GameDto>>(wishlist.Select(ug => ug.Game)));
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("User not authenticated.");

            var games = await _userGameService.GetFavoritesAsync(userId);
            return Ok(_mapper.Map<IEnumerable<GameDto>>(games.Select(ug => ug.Game)));
        }

        [HttpPut("{gameId}/favorite")]
        public async Task<IActionResult> ToggleFavorite(int gameId, [FromBody] bool isFavorite)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("User not authenticated.");

            var updated = await _userGameService.ToggleFavoriteAsync(userId, gameId, isFavorite);
            if (updated == null) return NotFound();

            return NoContent();
        }


    }
}
