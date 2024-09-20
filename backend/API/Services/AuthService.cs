using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public AuthService(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        // Registration logic
        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO)
        {
            if (await _userRepository.GetUserByUsernameAsync(registerDTO.Username) != null)
            {
                return (false, "The username is already taken");
            }

            var user = new User
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Password = HashPassword(registerDTO.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "You were successfully registered and can login now");
        }

        // Login logic
        public async Task SignInAsync(HttpContext httpContext, User user)
        {
            // Define the claims for the user - this is the information that will be stored in the cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // Create a claims identity (of the user) with the claims and the authentication scheme (cookie)
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                // Represents the user in the cookie
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    // cookie is valid for 30 minutes (persist even if browser is closed)
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });
        }

        // Authentication logic
        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(password, user.Password))
            {
                return null;
            }
            // Find the user and return it
            return user;
        }

        // Password hashing
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Password verification
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}