using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumStandUpTrackerProject.Services
{
    public interface IDailyStatusService
    {
        Task<IEnumerable<DailyStatusDTO>> GetAllAsync();
        Task<DailyStatusDTO?> GetByIdAsync(int id);
        Task<List<DailyStatusDTO>> GetByDeveloperNameAsync(string username);
        Task<List<DailyStatusDTO>> GetBySubmissionDateAsync(DateTime date);
        Task<DailyStatusDTO> AddAsync(DailyStatusDTO dto);
        Task<DailyStatus> UpdateDailyStatusAsync(int id, DailyStatusDTO dto);
        Task DeleteAsync(int id);
    }
}