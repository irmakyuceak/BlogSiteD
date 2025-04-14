using BlogSite.Helpers.Result;
using BlogSite.Models;

namespace BlogSite.Interfaces
{
    public interface ICommentService
    {
        Task<DataResult<Comment>> GetByIdAsync(int id);
        Task<DataResult<List<Comment>>> GetAllAsync();
        Task<DataResult<List<Comment>>> GetByArticleIdAsync(int articleId);
        Task<Result> AddAsync(Comment comment);
        Task<Result> UpdateAsync(Comment comment);
        Task<Result> DeleteAsync(int id);
    }
}
