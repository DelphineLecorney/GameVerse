using AutoMapper;
using FluentValidation;
using GameVerse.API.Extensions;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;
using GameVerse.SHARED.DTOs.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameVerse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateGameDto> _createValidator;
        private readonly IValidator<UpdateGameDto> _updateGameValidator;


        public GamesController(
            IGameService gameService,
            IMapper mapper, IValidator<CreateGameDto> createValidator,
            IValidator<UpdateGameDto> updateGameValidator)
        {
            _gameService = gameService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateGameValidator = updateGameValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _gameService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
                return NotFound();

            return Ok(_mapper.Map<GameDto>(game));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(GameDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGame(CreateGameDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.ToDictionary());

            var created = await _gameService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetGame),
                new { id = created.GameId },
                _mapper.Map<GameDto>(created));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GameDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateGame(int id, UpdateGameDto dto)
        {
            var validationResult = await _updateGameValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            var updated = await _gameService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(_mapper.Map<GameDto>(updated));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var deleted = await _gameService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpGet("library")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetUserLibrary()
        {
            var userId = User.GetUserId();

            if (userId == null)
                return BadRequest("User not authenticated.");

            var games = await _gameService.GetUserLibraryAsync(userId);

            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        [Authorize]
        [HttpDelete("library/{gameId}")]
        public async Task<IActionResult> RemoveFromLibrary(int gameId)
        {
            var userId = User.GetUserId();

            if (userId == null)
                return BadRequest("User not authenticated.");

            var removed = await _gameService.RemoveFromLibraryAsync(userId, gameId);

            if (!removed)
                return NotFound();

            return NoContent();
        }

    }
}
