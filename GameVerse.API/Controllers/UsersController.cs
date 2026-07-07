using AutoMapper;
using FluentValidation;
using GameVerse.SHARED.DTOs.Users;
using GameVerse.API.Services.Interfaces;
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
        {
            var validationResult = await _updateUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var updated = await _userService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(_mapper.Map<UserDto>(updated));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto dto)
        {
            var updated = await _userService.UpdateRoleAsync(id, dto.Role);

            if (!updated)
                return NotFound(new { message = "Utilisateur introuvable" });

            return NoContent();
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
