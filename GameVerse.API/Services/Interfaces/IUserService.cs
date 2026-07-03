using GameVerse.API.DTOs.Users;
using GameVerse.API.Models;

namespace GameVerse.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> UpdateRoleAsync(int userId, string role);
        Task<bool> DeleteAsync(int id);
    }
}
