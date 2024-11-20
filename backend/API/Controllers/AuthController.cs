using API.Interfaces;
using API.Models;
using API.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPut("auth")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? throw new InvalidOperationException("User ID not found in token"));

            var (success, message) = await _authService.ChangePasswordAsync(userId, changePasswordDTO, Response);

            if (success)
            {
                return Ok(new { message });
            }
            return BadRequest(new { message });
        }

        [HttpGet("is-logged-in")]
        [ProducesResponseType<object>(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsLoggedIn()
        {
            var token = Request.Cookies["token"];
            var (isLoggedIn, expiredPassword) = await _authService.CheckLoginStatusAsync(token);
            return Ok(new { isLoggedIn, expiredPassword });
        }
    }
}

