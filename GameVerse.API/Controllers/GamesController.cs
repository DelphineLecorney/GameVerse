using AutoMapper;
using GameVerse.API.DTOs.Games;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;
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

        public GamesController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _gameService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
                return NotFound();

            return Ok(_mapper.Map<GameDto>(game));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateGame(CreateGameDto dto)
        {
            var game = await _gameService.CreateAsync(dto);
            var result = _mapper.Map<GameDto>(game);

            return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, result);

        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, UpdateGameDto dto)
        {
            var updated = await _gameService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(_mapper.Map<GameDto>(updated));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var deleted = await _gameService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }
}
