using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;

namespace BlogSite.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<DataResult<Comment>> GetByIdAsync(int id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }

        public async Task<DataResult<List<Comment>>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<DataResult<List<Comment>>> GetByArticleIdAsync(int articleId)
        {
            return await _commentRepository.GetByArticleIdAsync(articleId);
        }

        public async Task<Result> AddAsync(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                return Result.Failure("Yorum içeriği boş olamaz.");
            }

            return await _commentRepository.AddAsync(comment);
        }

        public async Task<Result> UpdateAsync(Comment comment)
        {
            var existing = await _commentRepository.GetByIdAsync(comment.Id);
            if (!existing.IsSuccess)
            {
                return Result.Failure("Güncellenecek yorum bulunamadı.");
            }

            return await _commentRepository.UpdateAsync(comment);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _commentRepository.DeleteAsync(id);
        }
    }
}
