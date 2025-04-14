using BlogSite.Interfaces;
using BlogSite.Models;
using BlogSite.Models.View;
using BlogSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Comment/Create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Yorum geçersiz. Lütfen gerekli alanları doldurun.";
                return RedirectToAction("Details", "Article", new { id = comment.ArticleId });
            }

            var result = await _commentService.AddAsync(comment);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Yorum eklenirken bir hata oluştu.";
            }
            else
            {
                TempData["SuccessMessage"] = "Yorum başarıyla eklendi.";
            }

            return RedirectToAction("Details", "Article", new { id = comment.ArticleId });

        }
    }
}
