using BlogSite.Data;
using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResult<List<Category>>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return DataResult<List<Category>>.Success(categories);
        }

        public async Task<DataResult<Category>> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category != null
                ? DataResult<Category>.Success(category)
                : DataResult<Category>.Failure("Kategori bulunamadı.");
        }

        public async Task<Result> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return Result.Failure("Kategori bulunamadı.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}