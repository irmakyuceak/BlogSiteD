using BlogSite.Helpers.Result;
using BlogSite.Models;

namespace BlogSite.Interfaces
{
    public interface ICategoryService
    {
        Task<DataResult<List<Category>>> GetAllAsync();
        Task<DataResult<Category>> GetByIdAsync(int id);
        Task<Result> AddAsync(Category category);
        Task<Result> UpdateAsync(Category category);
        Task<Result> DeleteAsync(int id);
    }
}
