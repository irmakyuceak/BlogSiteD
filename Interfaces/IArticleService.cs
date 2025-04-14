using BlogSite.Helpers.Result;
using BlogSite.Models;

namespace BlogSite.Interfaces
{
    public interface IArticleService
    {
        Task<DataResult<Article>> GetByIdAsync(int id);
        Task<DataResult<List<Article>>> GetAllAsync();
        Task<Result> AddAsync(Article article, IFormFile imageFile);
        Task<Result> UpdateAsync(Article article, IFormFile imageFile, string? oldImagePath);
        Task<Result> DeleteAsync(int id);
    }
}
