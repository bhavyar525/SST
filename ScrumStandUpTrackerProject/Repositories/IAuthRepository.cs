using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;

namespace ScrumStandUpTrackerProject.Repositories
{
    public interface IAuthRepository
    {
        Task<DeveloperDTO> RegisterAsync(RegisterDTO dto);
        Task<object> LoginAsync(LoginDTO dto);
    
    }
}