using BlogSite.Helpers.Result;
using BlogSite.Models;

namespace BlogSite.Interfaces
{
    public interface IUserRepository
    {
        Task<DataResult<User>> GetByUsernameAsync(string username);
        Task<bool> IsUsernameTaken(string username);
        Task<Result> AddAsync(User user);
        Task<Result> UpdateAsync(User user);
        Task<DataResult<User>> GetByIdAsync(int id);
    }
}
