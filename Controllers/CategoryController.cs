using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using BlogSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("Category/Index")]
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Kategoriler yüklenemedi.";
                return View(new List<Category>());
            }

            return View(result.Data);
        }

        [HttpGet("Category/Create")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Category/Create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", category);
            }

            var result = await _categoryService.AddAsync(category);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Kategori eklenirken bir hata oluştu.";
                return View("Create", category);
            }

            TempData["SuccessMessage"] = "Kategori eklendi.";
            return RedirectToAction("Index", "Category");
        }
    }
}
