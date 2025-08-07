using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;

namespace ScrumStandUpTrackerProject.Repositories
{
    public interface IDailyStatusRepository
    {
        Task<IEnumerable<DailyStatus>> GetAllAsync();
        Task<DailyStatus> GetByIdAsync(int id);
        Task<DailyStatus> AddAsync(DailyStatus status);

        Task<DailyStatus> Update(int id,DailyStatus status);
        Task DeleteAsync(int id);
        Task<List<DailyStatus>> GetByDeveloperNameAsync(string username);
        Task<List<DailyStatus>> GetBySubmissionDateAsync(DateTime date);
    }
}