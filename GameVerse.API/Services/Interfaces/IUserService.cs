using GameVerse.SHARED.DTOs.Users;
using GameVerse.API.Models;

namespace GameVerse.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(string id);
        Task<User?> UpdateAsync(string id, UpdateUserDto dto);
        Task<bool> UpdateRoleAsync(string userId, string role);
        Task<bool> DeleteAsync(string id);
    }
}
