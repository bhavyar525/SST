using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;
using ScrumStandUpTrackerProject.Services;


    [ApiController]
    [Route("api/[controller]")]
    public class DailyStatusController : ControllerBase
    {
        private readonly IDailyStatusService _service;
        private readonly ILogger<DailyStatusController> _logger;
        public DailyStatusController(IDailyStatusService service, ILogger<DailyStatusController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<DailyStatusDTO>>> GetDailyStatuses()
        {
            _logger.LogInformation("Fetching all daily statuses.");
            var statuses = await _service.GetAllAsync();
            return Ok(statuses);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DailyStatusDTO>> GetDailyStatus(int id)
        {
            _logger.LogInformation("Fetching daily status with ID: {Id}", id);
            var results = await _service.GetByIdAsync(id);
            if (results == null) return NotFound();
            return Ok(results);
        }


        [HttpGet("developer/{name}")]
        public async Task<ActionResult<IEnumerable<DailyStatusDTO>>> GetByDeveloperName(string name)
        {
            _logger.LogInformation("Fetching daily statuses for developer: {Name}", name);
            var results = await _service.GetByDeveloperNameAsync(name);
            return Ok(results);
        }


        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<DailyStatusDTO>>> GetByDate(string date)
        {
            _logger.LogInformation("Fetching daily statuses for date: {Date}", date);
            if (!DateTime.TryParse(date, out var parsedDate))
            {
                _logger.LogWarning("Invalid date format provided: {Date}", date);
                return BadRequest("Invalid date format. Use YYYY-MM-DD.");
            }

            var results = await _service.GetBySubmissionDateAsync(parsedDate);
            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<DailyStatusDTO>> PostDailyStatus(DailyStatusDTO dto)
        {
            _logger.LogInformation("Creating new daily status for developer ID: {DeveloperId}", dto.DeveloperId);
            var createdDto = await _service.AddAsync(dto);
            _logger.LogInformation("Daily status created with ID: {Id}", createdDto.Id);
            return CreatedAtAction(nameof(GetDailyStatus), new { id = createdDto.Id }, createdDto);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<DailyStatus>> UpdateDailyStatusAsync(int id, [FromBody] DailyStatusDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID in route does not match ID in body.");
            }

            var updatedStatus = await _service.UpdateDailyStatusAsync(id, dto);
            if (updatedStatus == null)
            {
                return NotFound($"No status found with ID {id}");
            }
            return Ok(updatedStatus);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DailyStatus>> DeleteDailyStatus(int id)
        {
            _logger.LogInformation("Attempting to delete daily status with ID: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Daily status with ID {Id} not found for deletion.", id);
                return NotFound();
            }
            _logger.LogInformation("Successfully deleted daily status with ID: {Id}", id);
            return NoContent();
        }
    }