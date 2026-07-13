using AutoMapper;
using FluentValidation;
using GameVerse.API.Extensions;
using GameVerse.API.Services.Interfaces;
using GameVerse.SHARED.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameVerse.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IValidator<UpdateUserDto> updateUserValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _updateUserValidator = updateUserValidator;
        }

        [HttpPut("me")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMe(UpdateUserDto dto)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return BadRequest("User not authenticated.");

            var validationResult = await _updateUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            var updated = await _userService.UpdateAsync(userId, dto);
            if (updated == null)
                return NotFound();

            return Ok(_mapper.Map<UserDto>(updated));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(string id, UpdateRoleDto dto)
        {
            var updated = await _userService.UpdateRoleAsync(id, dto.Role);
            if (!updated)
                return NotFound(new { message = "Utilisateur introuvable" });

            return NoContent();
        }

        [HttpDelete("me")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMe()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return BadRequest("User not authenticated.");

            var deleted = await _userService.DeleteAsync(userId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}