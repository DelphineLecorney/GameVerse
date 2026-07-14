using GameVerse.API.Extensions;
using GameVerse.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameVerse.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IUserGameService _userGameService;

        public StatsController(IUserGameService userGameService)
        {
            _userGameService = userGameService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyStats()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return BadRequest("User not authenticated.");

            var stats = await _userGameService.GetStatsAsync(userId);
            return Ok(stats);
        }
    }
}