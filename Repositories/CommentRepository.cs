using BlogSite.Data;
using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResult<Comment>> GetByIdAsync(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Article)
                .FirstOrDefaultAsync(c => c.Id == id);

            return comment != null
                ? DataResult<Comment>.Success(comment)
                : DataResult<Comment>.Failure("Yorum bulunamadı.");
        }

        public async Task<DataResult<List<Comment>>> GetAllAsync()
        {
            var comments = await _context.Comments
                .Include(c => c.Article)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return DataResult<List<Comment>>.Success(comments);
        }

        public async Task<DataResult<List<Comment>>> GetByArticleIdAsync(int articleId)
        {
            var comments = await _context.Comments
                .Where(c => c.ArticleId == articleId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return DataResult<List<Comment>>.Success(comments);
        }

        public async Task<Result> AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return Result.Failure("Yorum bulunamadı.");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
