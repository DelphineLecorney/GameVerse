using GameVerse.API.Data;
using GameVerse.API.DTOs.Users;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;

namespace GameVerse.API.Services
{
    public class UserService : IUserService
    {
        private readonly GameVerseContext _context;

        public UserService(GameVerseContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            user.Username = dto.Username;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
