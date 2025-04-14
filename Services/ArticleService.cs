using BlogSite.Helpers;
using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using BlogSite.Repositories;

namespace BlogSite.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IWebHostEnvironment _env;

        public ArticleService(IArticleRepository articleRepository, IWebHostEnvironment env)
        {
            _articleRepository = articleRepository;
            _env = env;
        }

        public async Task<DataResult<List<Article>>> GetAllAsync()
        {
            return await _articleRepository.GetAllAsync();
        }

        public async Task<DataResult<Article>> GetByIdAsync(int id)
        {
            return await _articleRepository.GetByIdAsync(id);
        }

        public async Task<Result> AddAsync(Article article, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var fileName = await FileHelper.UploadFileAsync(imageFile, _env.WebRootPath);
                article.ImagePath = "/Articles/" + fileName;
            }

            return await _articleRepository.AddAsync(article);
        }

        public async Task<Result> UpdateAsync(Article article, IFormFile imageFile, string? oldImagePath)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = await FileHelper.UploadFileAsync(imageFile, _env.WebRootPath);

                // Mevcut görseli sil
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    var fullOldPath = Path.Combine(_env.WebRootPath, oldImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullOldPath))
                    {
                        System.IO.File.Delete(fullOldPath);
                    }
                }

                article.ImagePath = "/Articles/" + fileName;
            }

            return await _articleRepository.UpdateAsync(article);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _articleRepository.DeleteAsync(id);
        }
    }
}
