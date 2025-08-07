using ScrumStandUpTrackerProject.Models;

namespace ScrumStandUpTrackerProject.Services
{
    public interface ITokenService
    {
        string CreateToken(Developer developer);
    }
}