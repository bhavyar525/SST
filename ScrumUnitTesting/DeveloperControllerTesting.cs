////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using Moq;
////using ScrumStandUpTrackerProject.Controllers;
////using ScrumStandUpTrackerProject.DataLayer;
////using ScrumStandUpTrackerProject.DTOs;
////using ScrumStandUpTrackerProject.Models;
////using ScrumStandUpTrackerProject.Repositories;
////using ScrumStandUpTrackerProject.Services;
////using System.Collections.Generic;
////using System.Linq;

////[TestClass]
////public class DeveloperControllerTesting
////{
////    private Mock<IAuthService> _mockAuthService;
////    private AuthController _controller;

////    [TestInitialize]
////    public void Setup()
////    {
////        _mockAuthService = new Mock<IAuthService>();
////        _controller = new AuthController(_mockAuthService.Object);
////    }

////    [TestMethod]
////    public async Task Register_ReturnsOk()
////    {
////        // Arrange
////        var dto = new RegisterDTO
////        {
////            Username = "testuser",
////            Password = "Test@123"
////        };
////        object value = _mockAuthService.Setup(s => s.RegisterAsync(dto)).ReturnsAsync("Registered Successfully");

////        // Act
////        var result = await _controller.Register(dto);

////        // Assert
////        var okResult = result as OkObjectResult;
////        Assert.IsNotNull(okResult);
////        Assert.AreEqual(200, okResult.StatusCode);
////        Assert.AreEqual("Registered Successfully", okResult.Value);
////    }

////    [TestMethod]
////    public async Task Login_ReturnsOk()
////    {
////        // Arrange
////        var dto = new LoginDTO
////        {
////            Username = "testuser",
////            Password = "Test@123"
////        };
////        _mockAuthService.Setup(s => s.LoginAsync(dto)).ReturnsAsync("jwt-token");

////        // Act
////        var result = await _controller.Login(dto);

////        // Assert
////        var okResult = result as OkObjectResult;
////        Assert.IsNotNull(okResult);
////        Assert.AreEqual(200, okResult.StatusCode);
////        Assert.AreEqual("jwt-token", okResult.Value);
////    }

////}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using ScrumStandUpTrackerProject.Controllers;
//using ScrumStandUpTrackerProject.DTOs;
//using ScrumStandUpTrackerProject.Services;
//using System;
//using System.Threading.Tasks;

//namespace ScrumStandUpTrackerProject.Tests.Controllers
//{
//    [TestClass]
//    public class AuthControllerTests
//    {
//        private Mock<IAuthService> _authServiceMock;
//        private AuthController _authController;

//        [TestInitialize]
//        public void Setup()
//        {
//            _authServiceMock = new Mock<IAuthService>();
//            _authController = new AuthController(_authServiceMock.Object);
//        }

//        [TestMethod]
//        public async Task Register_ReturnsOkResult_WithRegisteredUser()
//        {
//            // Arrange
//            var registerDto = new RegisterDTO
//            {
//                UserName = "testuser",
//                Email = "test@example.com",
//                Password = "Password123!"
//            };

//            var registeredUser = new
//            {
//                Id = 1,
//                UserName = "testuser",
//                Email = "test@example.com"
//            };

//            _authServiceMock.Setup(s => s.RegisterAsync(registerDto))
//                            .ReturnsAsync(registeredUser);

//            // Act
//            var result = await _authController.Register(registerDto);

//            // Assert
//            var okResult = Assert.IsInstanceOfType(result, typeof(OkObjectResult)) as OkObjectResult;
//            Assert.IsNotNull(okResult);

//            dynamic value = okResult.Value;
//            Assert.AreEqual(registeredUser.Id, value.Id);
//            Assert.AreEqual(registeredUser.UserName, value.UserName);
//            Assert.AreEqual(registeredUser.Email, value.Email);
//        }

//        [TestMethod]
//        public async Task Register_ThrowsException_ReturnsBadRequest()
//        {
//            // Arrange
//            var registerDto = new RegisterDTO
//            {
//                UserName = "existinguser",
//                Email = "existing@example.com",
//                Password = "Password123!"
//            };

//            _authServiceMock.Setup(s => s.RegisterAsync(registerDto))
//                            .ThrowsAsync(new ArgumentException("Username already exists."));

//            // Act
//            var result = await _authController.Register(registerDto);

//            // Assert
//            var badRequestResult = Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult)) as BadRequestObjectResult;
//            Assert.IsNotNull(badRequestResult);

//            dynamic value = badRequestResult.Value;
//            Assert.AreEqual("Username already exists.", value.message);
//        }

//        [TestMethod]
//        public async Task Login_ReturnsOkResult_WithToken()
//        {
//            // Arrange
//            var loginDto = new LoginDTO
//            {
//                UserName = "testuser",
//                Password = "Password123!"
//            };

//            var tokenResponse = new
//            {
//                id = 1,
//                userName = "testuser",
//                token = "fakeJwtToken"
//            };

//            _authServiceMock.Setup(s => s.LoginAsync(loginDto))
//                            .ReturnsAsync(tokenResponse);

//            // Act
//            var result = await _authController.Login(loginDto);

//            // Assert
//            var okResult = Assert.IsInstanceOfType(result, typeof(OkObjectResult)) as OkObjectResult;
//            Assert.IsNotNull(okResult);

//            dynamic value = okResult.Value;
//            Assert.AreEqual(tokenResponse.id, value.id);
//            Assert.AreEqual(tokenResponse.userName, value.userName);
//            Assert.AreEqual(tokenResponse.token, value.token);
//        }

//        [TestMethod]
//        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
//        {
//            // Arrange
//            var loginDto = new LoginDTO
//            {
//                UserName = "testuser",
//                Password = "wrongpassword"
//            };

//            _authServiceMock.Setup(s => s.LoginAsync(loginDto))
//                            .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

//            // Act
//            var result = await _authController.Login(loginDto);

//            // Assert
//            var unauthorizedResult = Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult)) as UnauthorizedObjectResult;
//            Assert.IsNotNull(unauthorizedResult);

//            dynamic value = unauthorizedResult.Value;
//            Assert.AreEqual("Invalid credentials", value.message);
//        }
//    }
//}