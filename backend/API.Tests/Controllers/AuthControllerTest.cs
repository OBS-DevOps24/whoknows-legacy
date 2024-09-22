using API.Controllers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace API.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockLogger = new Mock<ILogger<AuthController>>();
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockLogger.Object, _mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123",
                ConfirmPassword = "password123"
            };
            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<RegisterDTO>()))
                .ReturnsAsync((true, "You were successfully registered and can login now"));

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
            Assert.Equal("You were successfully registered and can login now", message);
        }

        [Fact]
        public async Task Register_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Username = "existinguser",
                Email = "existing@example.com",
                Password = "password123",
                ConfirmPassword = "password123"
            };
            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<RegisterDTO>()))
                .ReturnsAsync((false, "The username is already taken"));

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var message = Assert.IsType<string>(badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value));
            Assert.Equal("The username is already taken", message);
        }

        [Fact]
        public async Task Register_PasswordsDoNotMatch_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123",
                ConfirmPassword = "password456"
            };
            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<RegisterDTO>()))
                .ReturnsAsync((false, "Passwords do not match"));

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var message = Assert.IsType<string>(badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value));
            Assert.Equal("Passwords do not match", message);
        }
    }
}