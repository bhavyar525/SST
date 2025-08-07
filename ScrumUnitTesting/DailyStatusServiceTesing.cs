using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;
using ScrumStandUpTrackerProject.Repositories;
using ScrumStandUpTrackerProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumStandUpTrackerProject.Services.Tests
{
    [TestClass]
    public class DailyStatusServiceTests
    {

        private Mock<IDailyStatusRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<DailyStatusService>> _mockLogger;
        private DailyStatusService _service; // real service instance

        // This method runs before each test in this class.
        // It creates fresh mocks and a new service instance for each test, ensuring tests run independently.
        [TestInitialize]
        public void Setup()
        {
            //Creating Mocks for Repos,Mappers,Logger Interface
            _mockRepository = new Mock<IDailyStatusRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DailyStatusService>>();

            _service = new DailyStatusService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object); // fake objects
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsMappedDTOs()
        {
            // Arrange
            var entities = new List<DailyStatus>
            {
                new DailyStatus
                {
                    Id = 1,
                    DeveloperId = 1,
                    DeveloperName = "Priya",
                    TaskDetails = "Worked on login feature",
                    DidYesterday = "Debugged code",
                    DoingToday = "Write tests",
                    Blockers = "None",
                    SubmissionDate = new DateTime(2025, 7, 31)
                }
            };

            var dtos = new List<DailyStatusDTO>
            {
                new DailyStatusDTO
                {
                    DeveloperId = 1,
                    DeveloperName = "Priya",
                    TaskDetails = "Worked on login feature",
                    DidYesterday = "Debugged code",
                    DoingToday = "Write tests",
                    Blockers = "None",
                    SubmissionDate = new DateTime(2025, 7, 31)
                }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mockMapper.Setup(m => m.Map<IEnumerable<DailyStatusDTO>>(entities)).Returns(dtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var first = result.First();
            Assert.AreEqual(1, first.DeveloperId);
            Assert.AreEqual("Priya", first.DeveloperName);
            Assert.AreEqual("Worked on login feature", first.TaskDetails);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsMappedDTO_WhenEntityExists()
        {
            // Arrange
            int id = 1;
            var entity = new DailyStatus
            {
                Id = id,
                DeveloperId = 1,
                DeveloperName = "Priya",
                TaskDetails = "Worked on login feature",
                DidYesterday = "Debugged code",
                DoingToday = "Write tests",
                Blockers = "None",
                SubmissionDate = new DateTime(2025, 7, 31)
            };
            var dto = new DailyStatusDTO
            {
                DeveloperId = 1,
                DeveloperName = "Priya",
                TaskDetails = "Worked on login feature",
                DidYesterday = "Debugged code",
                DoingToday = "Write tests",
                Blockers = "None",
                SubmissionDate = new DateTime(2025, 7, 31)
            };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<DailyStatusDTO>(entity)).Returns(dto);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DeveloperId);
            Assert.AreEqual("Priya", result.DeveloperName);
            Assert.AreEqual(new DateTime(2025, 7, 31), result.SubmissionDate);
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetByIdAsync_ReturnsNull_WhenEntityNotFound()
        {
            // Arrange
            int id = 99;
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ThrowsAsync(new KeyNotFoundException("DailyStatus Not Found"));

            // Act
            await _service.GetByIdAsync(id);

        }


        [TestMethod]
        public async Task AddAsync_CreatesAndReturnsDTO()
        {
            // Arrange
            var inputDto = new DailyStatusDTO
            {
                DeveloperId = 1,
                DeveloperName = "Priya",
                TaskDetails = "New task",
                DidYesterday = "Completed old task",
                DoingToday = "Start new feature",
                Blockers = "None"

            };

            var inputEntity = new DailyStatus
            {
                DeveloperId = 1,
                DeveloperName = "Priya",
                TaskDetails = "New task",
                DidYesterday = "Completed old task",
                DoingToday = "Start new feature",
                Blockers = "None"
            };

            var createdEntity = new DailyStatus
            {
                Id = 10,
                DeveloperId = 1,
                DeveloperName = "Priya",
                TaskDetails = "New task",
                DidYesterday = "Completed old task",
                DoingToday = "Start new feature",
                Blockers = "None",
                SubmissionDate = DateTime.UtcNow
            };

            var createdDto = new DailyStatusDTO
            {
                DeveloperId = 1,
                DeveloperName = "Priya",
                TaskDetails = "New task",
                DidYesterday = "Completed old task",
                DoingToday = "Start new feature",
                Blockers = "None",
                SubmissionDate = createdEntity.SubmissionDate
            };

            _mockMapper.Setup(m => m.Map<DailyStatus>(It.IsAny<DailyStatusDTO>())).Returns(inputEntity);
            _mockRepository.Setup(r => r.AddAsync(inputEntity)).ReturnsAsync(createdEntity);
            _mockMapper.Setup(m => m.Map<DailyStatusDTO>(createdEntity)).Returns(createdDto);

            // Act
            var result = await _service.AddAsync(inputDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Priya", result.DeveloperName);
            Assert.AreEqual("New task", result.TaskDetails);
            Assert.IsTrue(result.SubmissionDate <= DateTime.UtcNow); // SubmissionDate is roughly now
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepositoryDelete()
        {
            // Arrange
            int idToDelete = 5;
            _mockRepository.Setup(r => r.DeleteAsync(idToDelete)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _service.DeleteAsync(idToDelete);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(idToDelete), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task DeleteAsync_ReturnsNull_WhenEntityNotFound()
        {
            //Arrange
            int id = 999;
            //Stimulate the repo throwing an exception
            _mockRepository.Setup(r => r.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException("DailyStatus Not Found"));
            //Act
            await _service.DeleteAsync(id);
        }


        [TestMethod]

        public async Task GetByDevNameAsync_ReturnsMappedDTOs()
        {
            //Arrange
            string developerName = "Sagar";

            //Creating a Fake List of DailyStatus
            var entities = new List<DailyStatus>
            {
                new DailyStatus
                {
                    Id = 1,
                    DeveloperId = 101,
                    DeveloperName = developerName,
                    TaskDetails = "Test Task",
                    DoingToday = "Did A",
                    DidYesterday = "Doing B",
                    Blockers = "None",
                    SubmissionDate = DateTime.UtcNow
                }
            };
            //Creating a Fake List of DailyStatusDTO
            var dtos = new List<DailyStatusDTO>
            {
                new DailyStatusDTO
                {
                    DeveloperId = 101,
                    DeveloperName = developerName,
                    TaskDetails = "Test Task",
                    DoingToday = "Did A",
                    DidYesterday = "Doing B",
                    Blockers = "None",
                    SubmissionDate = entities[0].SubmissionDate,
                }
            };
            // Setup the repository mock to return the fake entities when called with the developer name
            _mockRepository.Setup(r => r.GetByDeveloperNameAsync(developerName)).ReturnsAsync(entities);
            //Setup the mapper mock to return DTOs from entities
            _mockMapper.Setup(m => m.Map<List<DailyStatusDTO>>(entities)).Returns(dtos);
            //Act (Call the real service method)
            var result = await _service.GetByDeveloperNameAsync(developerName);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(developerName, result[0].DeveloperName);

        }

        
        [TestMethod]
        public async Task GetBySubDateAsync_ReturnsMappedDTOs()
        {
            //Arrange
            var date = new DateTime(2025, 8, 1);
            //Data from DB
            var entities = new List<DailyStatus>
            {
                new DailyStatus
                {
                    Id = 2,
                    DeveloperId = 202,
                    DeveloperName = "Ravi",
                    TaskDetails = "Sample task",
                    DidYesterday = "Worked on bugs",
                    DoingToday = "Refactoring",
                    Blockers = "None",
                    SubmissionDate = date
                }
            };
            //Stimulated Mapped DTOs
            var dtos = new List<DailyStatusDTO>
            {
                new DailyStatusDTO
                {
                    DeveloperId = 202,
                    DeveloperName = "Ravi",
                    TaskDetails = "Sample task",
                    DidYesterday = "Worked on bugs",
                    DoingToday = "Refactoring",
                    Blockers = "None",
                    SubmissionDate = date
                }
            };
            _mockRepository.Setup(r => r.GetBySubmissionDateAsync(date)).ReturnsAsync(entities);
            _mockMapper.Setup(m => m.Map<List<DailyStatusDTO>>(entities)).Returns(dtos);

            //Act
            var result = await _service.GetBySubmissionDateAsync(date);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(date, result.First().SubmissionDate);
            Assert.AreEqual(date,result[0].SubmissionDate);
            }
    }
    }
