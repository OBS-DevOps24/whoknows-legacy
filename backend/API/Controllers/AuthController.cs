﻿using API.Interfaces;
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
            var token = await _authService.SignInAsync(user);

            // Set the token in the response headers
            Response.Headers.Add("Authorization", $"Bearer {token}");
            return Ok(new { message = "Logged in successfully" });
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "No token provided" });
            }

            await _authService.SignOutAsync(token);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}

