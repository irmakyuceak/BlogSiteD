using BlogSite.Data;
using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResult<User>> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user != null
                ? DataResult<User>.Success(user)
                : DataResult<User>.Failure("Kullanıcı bulunamadı.");
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<Result> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<DataResult<User>> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user != null
                ? DataResult<User>.Success(user)
                : DataResult<User>.Failure("Kullanıcı bulunamadı.");
        }
    }
}