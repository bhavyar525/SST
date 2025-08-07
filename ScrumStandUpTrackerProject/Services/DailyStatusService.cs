using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;
using ScrumStandUpTrackerProject.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScrumStandUpTrackerProject.Services
{
    public class DailyStatusService:IDailyStatusService
    {
        private readonly IDailyStatusRepository _repository;
        private readonly ILogger<DailyStatusService> _logger;
        private readonly IMapper _mapper;
        public DailyStatusService(IDailyStatusRepository repository,IMapper mapper, ILogger<DailyStatusService> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DailyStatusDTO>> GetAllAsync()
        {
            _logger.LogInformation("Service: Fetching all DailyStatus records"); 
            //Getting DS entities from repo's
            var status = await _repository.GetAllAsync();
            //Map entities to DTO's
            var statusDTO = _mapper.Map<IEnumerable<DailyStatusDTO>>(status);
            return statusDTO;
        }

        public async Task<DailyStatusDTO?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Service: Fetching daily status with ID {Id}", id);

            //Getting the single record from repo's
            var status=  await _repository.GetByIdAsync(id);
            //Map entities to DTO's
            var statusDTO = _mapper.Map<DailyStatusDTO>(status);
            return statusDTO;
        }

        public async Task<List<DailyStatusDTO>> GetByDeveloperNameAsync(string username)
        {
            _logger.LogInformation("Service: Fetching daily statuses for developer {DeveloperName}", username);
            var entities = await _repository.GetByDeveloperNameAsync(username);
            //Map Entities to DTOs
            var dtos = _mapper.Map<List<DailyStatusDTO>>(entities);
            return dtos;
        }

        public async Task<List<DailyStatusDTO>> GetBySubmissionDateAsync(DateTime date)
        {
            _logger.LogInformation("Service: Fetching daily statuses for date {Date}", date);
            var entities = await _repository.GetBySubmissionDateAsync(date);
            var dtos = _mapper.Map<List<DailyStatusDTO>>(entities);
            return dtos;
        }
        

        public async Task<DailyStatusDTO> AddAsync(DailyStatusDTO dto)
        {
            _logger.LogInformation("Service: Adding new daily status for developer {DeveloperName}", dto.DeveloperName);
           
            var entity = _mapper.Map<DailyStatus>(dto); // Dto- entity
            entity.SubmissionDate = DateTime.UtcNow;
            //var status = new DailyStatus
            //{
            //    DeveloperId = dto.DeveloperId,
            //    DeveloperName = dto.DeveloperName,
            //    TaskDetails = dto.TaskDetails,
            //    DidYesterday = dto.DidYesterday,
            //    DoingToday = dto.DoingToday,
            //    Blockers = dto.Blockers,
            //    SubmissionDate = DateTime.UtcNow
            //};


            var createdEntity = await _repository.AddAsync(entity); 

            var createdDto = _mapper.Map<DailyStatusDTO>(createdEntity);  
            return createdDto;
        }



        public async Task<DailyStatus> UpdateDailyStatusAsync(int id, DailyStatusDTO dto)
        {
            // Map DTO to entity manually, handle nullable SubmissionDate safely
            //var entity = new DailyStatus
            //{
            //    DeveloperId = dto.DeveloperId,
            //    DeveloperName = dto.DeveloperName,
            //    TaskDetails = dto.TaskDetails,
            //    DidYesterday = dto.DidYesterday,
            //    DoingToday = dto.DoingToday,
            //    Blockers = dto.Blockers,
            //    SubmissionDate = dto.SubmissionDate.Value
            //};
            var entity = _mapper.Map<DailyStatus>(dto);
            var updatedEntity = await _repository.Update(id, entity);
            return updatedEntity;
        }


        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleted {id}");
            await _repository.DeleteAsync(id);
        }
    }
}