using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScrumStandUpTrackerProject.DataLayer;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;
using ScrumStandUpTrackerProject.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AuthRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Called from AuthService *after* validation & password hashing
    public async Task<DeveloperDTO> RegisterAsync(RegisterDTO dto)
    {
        var developer = new Developer
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PasswordHash = dto.Password // Already hashed by service
        };

        _context.Developers.Add(developer);
        await _context.SaveChangesAsync();

        return _mapper.Map<DeveloperDTO>(developer);
    }

    // Only fetches developer, no logic
    public async Task<object> LoginAsync(LoginDTO dto)
    {
        var user = await _context.Developers.FirstOrDefaultAsync(x => x.UserName == dto.UserName);
        return user;
    }
}
