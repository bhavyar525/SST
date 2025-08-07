using ScrumStandUpTrackerProject.DTOs;


namespace ScrumStandUpTrackerProject.Services
{
    public interface IAuthService
    {
        Task<DeveloperDTO> RegisterAsync(RegisterDTO dto);
        Task<object> LoginAsync(LoginDTO dTO);
    }
}