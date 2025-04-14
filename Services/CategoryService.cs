using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using BlogSite.Repositories;

namespace BlogSite.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<DataResult<List<Category>>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<DataResult<Category>> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Result> AddAsync(Category category)
        {
            return await _categoryRepository.AddAsync(category);
        }

        public async Task<Result> UpdateAsync(Category category)
        {
            return await _categoryRepository.UpdateAsync(category);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }
    }
}