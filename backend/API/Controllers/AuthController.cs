using API.Interfaces;
using API.Models;
using API.Models.Dtos;
using API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                return Ok(new { message });
            }
            else
            {
                return BadRequest(new { message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _authService.AuthenticateUserAsync(loginDTO.Username, loginDTO.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid username or password" });
            }

            await _authService.SignInAsync(HttpContext, user);
            return Ok(new { message = "Logged in successfully" });
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync(HttpContext);
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("is-logged-in")]
        public IActionResult IsLoggedIn()
        {
            return Ok(new { isLoggedIn = User.Identity.IsAuthenticated });
        }
    }
}

