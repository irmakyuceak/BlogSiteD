using Microsoft.AspNetCore.Mvc;
using BlogSite.Services;
using BlogSite.Models;
using BlogSite.Models.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BlogSite.Models.View;
using Microsoft.AspNetCore.Authorization;
using EtkinlikPlatformu.Helpers;
using BlogSite.Interfaces;

namespace BlogSite.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IArticleService _articleService;

        public UserController(IUserService userService, IArticleService articleService)
        {
            _userService = userService;
            _articleService = articleService;
        }

        [HttpGet("User/Login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost("User/Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View("Login", loginDto);

            var result = await _userService.LoginAsync(loginDto);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Login");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Data.Username),
                new Claim(ClaimTypes.Role, result.Data.Role),
                new Claim("Id", result.Data.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("User/Register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("User/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View("Register", dto);

            var result = await _userService.RegisterAsync(dto);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View("Register");
            }

            TempData["SuccessMessage"] = "Kayıt Oluşturuldu.";
            return RedirectToAction("Login");
        }

        [HttpGet("User/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet("User/Profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            int userId = int.Parse(User.FindFirst("Id")!.Value);

            var userResult = await _userService.GetByIdAsync(userId);
            if (!userResult.IsSuccess || userResult.Data == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bilgileri getirilemedi.";
                return RedirectToAction("Index", "Home");
            }

            var allArticles = await _articleService.GetAllAsync();
            var userArticles = allArticles.Data
                .Where(a => a.UserId == userId)
                .ToList();

            var viewModel = new UserProfileViewModel
            {
                User = userResult.Data,
                Articles = userArticles
            };

            return View("Profile", viewModel);
        }

        [HttpPost("User/UpdateProfile")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string Username, string Email, string Password)
        {
            int userId = int.Parse(User.FindFirst("Id")!.Value);

            var userResult = await _userService.GetByIdAsync(userId);
            if (!userResult.IsSuccess || userResult.Data == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Profile");
            }

            var user = userResult.Data;

            user.Username = Username;
            user.Email = Email;

            if (!string.IsNullOrWhiteSpace(Password))
            {
                user.Password = PasswordHelper.HashPassword(Password);
            }

            var updateResult = await _userService.UpdateAsync(user);
            if (!updateResult.IsSuccess)
            {
                TempData["ErrorMessage"] = "Güncelleme sırasında hata oluştu.";
            }
            else
            {
                TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
            }

            return RedirectToAction("Profile");
        }

        [HttpGet("Article/Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var articleResult = await _articleService.GetByIdAsync(id);

            if (!articleResult.IsSuccess || articleResult.Data == null)
            {
                TempData["ErrorMessage"] = "Blog yazısı bulunamadı.";
                return RedirectToAction("Profile", "User");
            }

            var article = articleResult.Data;

            int userId = int.Parse(User.FindFirst("Id")!.Value);
            if (article.UserId != userId)
            {
                TempData["ErrorMessage"] = "Bu blog yazısını silmeye yetkiniz yok.";
                return RedirectToAction("Profile", "User");
            }

            var deleteResult = await _articleService.DeleteAsync(id);

            if (!deleteResult.IsSuccess)
            {
                TempData["ErrorMessage"] = "Silme işlemi sırasında bir hata oluştu.";
            }
            else
            {
                TempData["SuccessMessage"] = "Blog yazısı başarıyla silindi.";
            }

            return RedirectToAction("Profile", "User");
        }
    }
}