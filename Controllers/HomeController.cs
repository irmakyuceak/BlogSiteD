using System.Diagnostics;
using BlogSite.Data;
using BlogSite.Interfaces;
using BlogSite.Models;
using BlogSite.Models.View;
using BlogSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public HomeController(
            ILogger<HomeController> logger,
            IArticleService articleService,
            ICategoryService categoryService)
        {
            _logger = logger;
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var articlesResult = await _articleService.GetAllAsync();
            var categoriesResult = await _categoryService.GetAllAsync();

            if (!articlesResult.IsSuccess || !categoriesResult.IsSuccess)
            {
                TempData["ErrorMessage"] = "Veriler yüklenemedi.";
                return View(new ArticleIndexViewModel());
            }

            var articles = articlesResult.Data;

            if (categoryId.HasValue)
                articles = articles.Where(a => a.CategoryId == categoryId.Value).ToList();

            var viewModel = new ArticleIndexViewModel
            {
                Articles = articles,
                Categories = categoriesResult.Data,
                SelectedCategoryId = categoryId
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}