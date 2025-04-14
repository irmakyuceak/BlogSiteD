using BlogSite.Helpers.Result;
using BlogSite.Models;

namespace BlogSite.Interfaces
{
    public interface IArticleRepository
    {
        Task<DataResult<Article>> GetByIdAsync(int id);
        Task<DataResult<List<Article>>> GetAllAsync();
        Task<Result> AddAsync(Article article);
        Task<Result> UpdateAsync(Article article);
        Task<Result> DeleteAsync(int id);
    }
}
