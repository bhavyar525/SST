using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScrumStandUpTrackerProject.DataLayer;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;

namespace ScrumStandUpTrackerProject.Repositories
{
    public class DailyStatusRepository : IDailyStatusRepository
    {
        private readonly ILogger<DailyStatusRepository> _logger;
        private readonly ApplicationDbContext _context;
        public DailyStatusRepository(ApplicationDbContext context,ILogger<DailyStatusRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<DailyStatus>> GetAllAsync(){

            _logger.LogInformation("Fetching all DailyStatus records");
            return await _context.DailyStatuses.Include(d => d.Developer).ToListAsync();
        }

        public async Task<DailyStatus> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Getting DailyStatus by ID: {id}");
           
            var status = await _context.DailyStatuses.FindAsync(id);
            if(status == null)
            {

                _logger.LogWarning("DailyStatus with ID {Id} not found", id);
                throw new KeyNotFoundException("DailyStatus Not Found");
            }
            return status;
        }

        public async Task<DailyStatus> AddAsync(DailyStatus status)
        {
            _logger.LogInformation("Adding new DailyStatus for DeveloperId: {DeveloperId}",status.DeveloperId);
            _context.DailyStatuses.Add(status);
            await _context.SaveChangesAsync();
            _logger.LogInformation("DailyStatus added successfully with ID: {Id}", status.Id);
            
            return status;
        }
        public async Task<List<DailyStatus>> GetByDeveloperNameAsync(string developerName)
        {
            _logger.LogInformation("Fetching DailyStatus records for DeveloperName: {Name}", developerName);
           
            var status = await _context.DailyStatuses.Where(ds => ds.DeveloperName.ToLower() == developerName.ToLower()).ToListAsync();
            if(status == null || status.Count == 0)
            {

                _logger.LogWarning("DailyStatus with developer Name {developerName} not found", developerName);
                throw new KeyNotFoundException($"No DailyStatus records found for developer: {developerName}");
            }
            return status;
        }

        public async Task<List<DailyStatus>> GetBySubmissionDateAsync(DateTime date)
        {
            _logger.LogInformation("Fetching DailyStatus records for SubmissionDate: {Date}", date.Date);
            return await _context.DailyStatuses.Where(ds => ds.SubmissionDate.Date == date.Date).ToListAsync();
        }

        public async Task<DailyStatus> Update(int id,DailyStatus status)
        {
            var s = await _context.DailyStatuses.FindAsync(id);
           if(s == null)
            {
                throw new KeyNotFoundException("DailyStatus with id is not found");
            }
            s.DeveloperId = status.DeveloperId;
            s.DeveloperName = status.DeveloperName;
            s.TaskDetails = status.TaskDetails;
            s.DidYesterday = status.DidYesterday;
            s.DoingToday = status.DoingToday;
            s.Blockers = status.Blockers;
            s.SubmissionDate = status.SubmissionDate;
            await _context.SaveChangesAsync();
            return s;
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting DailyStatus with ID: {Id}", id);
            var status = await _context.DailyStatuses.FindAsync(id);
            if (status == null)
            {
                _logger.LogWarning("DailyStatus with ID {Id} not found", id);
                throw new KeyNotFoundException("DailyStatus not found");
            }
            _context.DailyStatuses.Remove(status);
            await _context.SaveChangesAsync();
            _logger.LogInformation("DailyStatus with ID {Id} deleted successfully", id);
        }

    }
}
