using BlogSite.Helpers.Result;
using BlogSite.Models.Dto;
using BlogSite.Models;

namespace BlogSite.Interfaces
{
    public interface IUserService
    {
        Task<DataResult<User>> LoginAsync(LoginDto loginDto);
        Task<Result> RegisterAsync(RegisterDto registerDto);
        Task<Result> UpdateAsync(User user);
        Task<DataResult<User>> GetByIdAsync(int userId);
    }
}
