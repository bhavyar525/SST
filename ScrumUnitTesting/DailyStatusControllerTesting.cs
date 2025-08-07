using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ScrumStandUpTrackerProject.Models;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumUnitTesting
{
    [TestClass]
    public class DailyStatusControllerTesting
    {
        private Mock<IDailyStatusService> _mockService;
        private Mock<ILogger<DailyStatusController>> _mockLogger;
        private DailyStatusController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IDailyStatusService>();
            _mockLogger = new Mock<ILogger<DailyStatusController>>();
            _controller = new DailyStatusController(_mockService.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetDailyStatuses_ReturnsOkResultWithList()
        {
            // Arrange
            var data = new List<DailyStatusDTO> {
                new DailyStatusDTO { Id = 1, DeveloperId = 1, TaskDetails = "Work A" },
                new DailyStatusDTO { Id = 2, DeveloperId = 2, TaskDetails = "Work B" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(data);

            // Act
            var result = await _controller.GetDailyStatuses();
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            var statuses = okResult.Value as IEnumerable<DailyStatusDTO>;
            Assert.AreEqual(2, statuses.Count());
        }

        [TestMethod]
        public async Task GetDailyStatus_ReturnsOk_WhenFound()
        {
            var dto = new DailyStatusDTO { Id = 1, DeveloperId = 1, TaskDetails = "Work" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetDailyStatus(1);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(dto, okResult.Value);
        }

        [TestMethod]
        public async Task GetDailyStatus_ReturnsNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(10)).ReturnsAsync((DailyStatusDTO)null);

            var result = await _controller.GetDailyStatus(10);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostDailyStatus_ReturnsCreatedAtAction()
        {
            var dto = new DailyStatusDTO { Id = 1, DeveloperId = 1, TaskDetails = "Work" };
            _mockService.Setup(s => s.AddAsync(dto)).ReturnsAsync(dto);

            var result = await _controller.PostDailyStatus(dto);
            var createdResult = result.Result as CreatedAtActionResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetDailyStatus", createdResult.ActionName);
            Assert.AreEqual(dto, createdResult.Value);
        }

        [TestMethod]
        public async Task UpdateDailyStatus_ReturnsOk_WhenSuccessful()
        {
            var dto = new DailyStatusDTO { Id = 1, DeveloperId = 1, TaskDetails = "Updated" };
            _mockService.Setup(s => s.UpdateDailyStatusAsync(1, dto)).ReturnsAsync(new ScrumStandUpTrackerProject.Models.DailyStatus());

            var result = await _controller.UpdateDailyStatusAsync(1, dto);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task UpdateDailyStatus_ReturnsNotFound_WhenNull()
        {
            var dto = new DailyStatusDTO { Id = 2, DeveloperId = 1, TaskDetails = "Updated" };
            _mockService.Setup(s => s.UpdateDailyStatusAsync(2, dto)).ReturnsAsync((ScrumStandUpTrackerProject.Models.DailyStatus)null);

            var result = await _controller.UpdateDailyStatusAsync(2, dto);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteDailyStatus_ReturnsNoContent_WhenDeleted()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteDailyStatus(1);
            Assert.IsInstanceOfType(result.Result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteDailyStatus_ReturnsNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Throws(new KeyNotFoundException());

            var result = await _controller.DeleteDailyStatus(1);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

    }
}