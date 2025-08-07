using Microsoft.EntityFrameworkCore;
using ScrumStandUpTrackerProject.DataLayer;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;
using ScrumStandUpTrackerProject.Repositories;

namespace ScrumStandUpTrackerProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public AuthService(IAuthRepository authRepo, ITokenService tokenService, ApplicationDbContext context)
        {
            _authRepo = authRepo;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<DeveloperDTO> RegisterAsync(RegisterDTO dto)
        {
            var emailExists = await _context.Developers.AnyAsync(d => d.Email.ToLower() == dto.Email.ToLower());
            var usernameExists = await _context.Developers.AnyAsync(d => d.UserName.ToLower() == dto.UserName.ToLower());

            if (emailExists) throw new ArgumentException("Email already exists.");
            if (usernameExists) throw new ArgumentException("Username already exists.");

            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password); // hash before passing

            return await _authRepo.RegisterAsync(dto);
        }

        public async Task<object> LoginAsync(LoginDTO dto)
        {
            var result = await _authRepo.LoginAsync(dto);

            if (result is not Developer user || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            return new
            {
                id = user.Id,
                userName = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }
    }
}