using API.Interfaces;
using API.Models;
using API.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            (bool success, string message) = await _authService.RegisterAsync(registerDTO, Response);
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
            (bool success, string message) = await _authService.LoginAsync(loginDTO, Response);
            if (success)
            {
                return Ok(new { message });
            }
            else
            {
                return BadRequest(new { message });
            }
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "No token provided" });
            }
            (bool success, string message) = await _authService.LogoutAsync(token, Response);
            if (success)
            {
                return Ok(new { message });
            }
            else
            {
                return BadRequest(new { message });
            }
        }

        [HttpGet("is-logged-in")]
        public IActionResult IsLoggedIn()
        {
            var token = Request.Cookies["token"];
            return Ok(new { isLoggedIn = !string.IsNullOrEmpty(token) });
        }
    }
}

