using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using BlogSite.Models.View;
using BlogSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public ArticleController(IArticleService articleService, ICategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [HttpGet("Article/Index")]
        public async Task<IActionResult> Index()
        {
            var result = await _articleService.GetAllAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Blog yazıları yüklenemedi.";
                return View(new List<Article>());
            }

            return View(result.Data);
        }

        [HttpGet("Article/Create")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();

            var articleCreateView = new ArticleCreateViewModel
            {
                Categories = categories.Data
            };
            return View(articleCreateView);
        }

        [HttpPost("Article/Create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ArticleCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                viewModel.Categories = categories.Data;
                return View("Create", viewModel);
            }

            var article = viewModel.Article!;
            article.UserId = int.Parse(User.FindFirst("Id")!.Value);
            article.PublishDate = DateTime.Now;

            // Görsel dosyayı servis metoduna gönderiyoruz
            var result = await _articleService.AddAsync(article, viewModel.ImageFile);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Blog yazısı eklenirken bir hata oluştu.";
                var categories = await _categoryService.GetAllAsync();
                viewModel.Categories = categories.Data;
                return View("Create", viewModel);
            }

            TempData["SuccessMessage"] = "Blog Yazısı Eklendi.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Article/Details/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _articleService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound();
            }

            return View("Details", result.Data);
        }

        [HttpGet("Article/Edit/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int id)
        {
            var articleResult = await _articleService.GetByIdAsync(id);
            var categoryResult = await _categoryService.GetAllAsync();

            if (!articleResult.IsSuccess || !categoryResult.IsSuccess)
            {
                return NotFound();
            }

            var articleCreateView = new ArticleCreateViewModel
            {
                Categories = categoryResult.Data,
                Article = articleResult.Data
            };

            return View("Edit", articleCreateView);
        }

        [HttpPost("Article/Update")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Update(ArticleCreateViewModel viewModel)
        {
            var article = viewModel.Article!;
            article.UserId = int.Parse(User.FindFirst("Id")!.Value);
            article.PublishDate = DateTime.Now;

            var result = await _articleService.UpdateAsync(article, viewModel.ImageFile, viewModel.ExistingImagePath);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Blog yazısı güncellenirken bir hata oluştu.";
                var categories = await _categoryService.GetAllAsync();
                viewModel.Categories = categories.Data;
                return View("Edit", viewModel);
            }

            TempData["SuccessMessage"] = "Blog Yazısı Güncellendi.";
            return RedirectToAction("Profile", "User");
        }
    }
}
