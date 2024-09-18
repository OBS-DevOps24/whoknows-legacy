using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            (bool success, string message) = await _authService.RegisterAsync(registerDTO);
            if (success)
            {
                _logger.LogInformation("User registered successfully");
                return Ok(new { message });
            }
            else
            {
                _logger.LogInformation("Registration attempt failed");
                return BadRequest(new { message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] object loginDto)
        {
            // Login logic goes here
            _logger.LogInformation("Login attempt");
            return Ok(new { token = "dummy_jwt_token", userId = 1 });
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            // Logout logic goes here
            _logger.LogInformation("Logout attempt");
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
