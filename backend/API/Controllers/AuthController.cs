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
        private readonly ILoggerFactory _loggerFactory;
        public AuthController(IAuthService authService, ILoggerFactory loggerFactory)
        {
            _authService = authService;
            _loggerFactory = loggerFactory;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var logger = _loggerFactory.CreateLogger<AuthController>();

            // Log request details
            logger.LogInformation("Register attempt - Content-Type: {contentType}, Content-Length: {contentLength}",
                Request.ContentType ?? "no content type",
                Request.ContentLength ?? 0
            );

            // Log the request body
            try {
                Request.EnableBuffering();  // Add this
                var body = await new StreamReader(Request.Body).ReadToEndAsync();
                logger.LogInformation("Request body: {body}", body);
                Request.Body.Position = 0;  // Add this
            }
            catch (Exception ex) {
                logger.LogError(ex, "Error reading request body");
            }
            
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
        [ProducesResponseType<object>(StatusCodes.Status200OK)]
        public IActionResult IsLoggedIn()
        {
            var token = Request.Cookies["token"];
            return Ok(new { isLoggedIn = !string.IsNullOrEmpty(token) });
        }
    }
}

