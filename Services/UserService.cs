using BlogSite.Data;
using BlogSite.Helpers;
using BlogSite.Helpers.Result;
using BlogSite.Interfaces;
using BlogSite.Models;
using BlogSite.Models.Dto;
using BlogSite.Repositories;
using EtkinlikPlatformu.Helpers;
using Microsoft.AspNetCore.Http;

namespace BlogSite.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<DataResult<User>> LoginAsync(LoginDto loginDto)
        {
            var kullaniciResult = await _userRepository.GetByUsernameAsync(loginDto.Username);

            if (!kullaniciResult.IsSuccess)
                return DataResult<User>.Failure("Kullanıcı bulunamadı.");

            var kullanici = kullaniciResult.Data;
            var verifyPassword = PasswordHelper.VerifyPassword(loginDto.Password, kullanici.Password);

            if (!verifyPassword)
                return DataResult<User>.Failure("Şifre hatalı!");

            return DataResult<User>.Success(kullanici);
        }

        public async Task<Result> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userRepository.IsUsernameTaken(registerDto.Username))
                return Result.Failure("Bu kullanıcı adı zaten kullanılıyor.");

            var user = new User
            {
                Username = registerDto.Username,
                Password = PasswordHelper.HashPassword(registerDto.Password),
                Email = registerDto.Email,
                Role = "User"
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task<Result> UpdateAsync(User user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<DataResult<User>> GetByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }
    }
}