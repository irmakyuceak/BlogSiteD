using BlogSite.Data;
using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResult<Article>> GetByIdAsync(int id)
        {
            var article = await _context.Articles
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.Comments) // Yorumları dahil et
                .FirstOrDefaultAsync(a => a.Id == id);

            return article != null
                ? DataResult<Article>.Success(article)
                : DataResult<Article>.Failure("Yazı bulunamadı.");
        }

        public async Task<DataResult<List<Article>>> GetAllAsync()
        {
            var articles = await _context.Articles
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.Comments) // Eğer yorum sayısı önemliyse
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();

            return DataResult<List<Article>>.Success(articles);
        }

        public async Task<Result> AddAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return Result.Failure("Yazı bulunamadı.");

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
