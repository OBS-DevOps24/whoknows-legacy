using API.Data;
using API.Interfaces;
using API.Models;
using API.Models.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRedisService _redisService;

        public AuthService(DataContext context, IUserRepository userRepository, IConfiguration configuration, IRedisService redisService)
        {
            _context = context;
            _userRepository = userRepository;
            _configuration = configuration;
            _redisService = redisService;
        }

        // Registration logic
        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO, HttpResponse response)
        {
            // Custom email validation, to ensure proper feedback to the user
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var timeout = TimeSpan.FromSeconds(1);
            if (!System.Text.RegularExpressions.Regex.IsMatch(registerDTO.Email, emailPattern, System.Text.RegularExpressions.RegexOptions.None, timeout))
            {
                return (false, "Invalid email address");
            }
            if (await _userRepository.GetUserByUsernameAsync(registerDTO.Username) != null)
            {
                return (false, "The username is already taken");
            }
            if (!string.IsNullOrEmpty(registerDTO.Password2) && registerDTO.Password != registerDTO.Password2)
            {
                return (false, "Passwords do not match");
            }
            if (await _userRepository.GetUserByEmailAsync(registerDTO.Email) != null)
            {
                return (false, "The email is already registered");
            }

            var user = new User
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Password = HashPassword(registerDTO.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Auto login after registration
            var loginDTO = new LoginDTO { Username = registerDTO.Username, Password = registerDTO.Password };
            (bool loginSuccess, string loginMessage) = await LoginAsync(loginDTO, response);

            return (loginSuccess
                ? (true, "You were successfully registered and logged in")
                : (false, "You were successfully registered but could not be logged in"));
        }

        // Login logic
        public async Task<(bool Success, string Message)> LoginAsync(LoginDTO loginDTO, HttpResponse response)
        {
            var user = await AuthenticateUserAsync(loginDTO.Username, loginDTO.Password);
            if (user == null)
            {
                return (false, "Invalid username or password");
            }
            GenerateAndSetHttpOnlyCookie(user, response);
            return (true, "Logged in successfully");
        }


        // Logout logic
        public async Task<(bool Success, string Message)> LogoutAsync(string token, HttpResponse response)
        {
            if (string.IsNullOrEmpty(token))
            {
                return (false, "No token provided");
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                var jti = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "jti")?.Value;
                if (!string.IsNullOrEmpty(jti))
                {
                    var expiration = jsonToken.ValidTo;
                    var timeToLive = expiration - DateTime.UtcNow;

                    if (timeToLive > TimeSpan.Zero)
                    {
                        await _redisService.AddToBlacklistAsync(jti, timeToLive);
                    }
                }
            }

            response.Cookies.Delete("token");
            return (true, "Logged out successfully");
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

        // Change Password logic
        public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDTO, HttpResponse response)
        {
            try
            {
                // Find the user
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return (false, "User not found");
                }
                // Check if the old password is correct
                if (!VerifyPassword(changePasswordDTO.OldPassword, user.Password))
                {
                    return (false, "Current password is incorrect");
                }
                // Check if the new password is the same as the old password
                if (VerifyPassword(changePasswordDTO.NewPassword, user.Password))
                {
                    return (false, "New password cannot be the same as the old password");
                }
                // Change the password and set expired flag to false
                user.Password = HashPassword(changePasswordDTO.NewPassword);
                user.ExpiredPassword = false;
                await _context.SaveChangesAsync();

                return (true, "Password changed successfully");
            }
            catch (Exception)
            {
                return (false, "An error occurred while changing the password");
            }
        }

        // Check if the user is logged in and if the password has expired
        public async Task<(bool IsLoggedIn, bool ExpiredPassword)> CheckLoginStatusAsync(string token)
        {
            // Check if the token is empty
            if (string.IsNullOrEmpty(token))
            {
                return (false, false);
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                var userId = int.Parse(jsonToken.Claims.First(claim => claim.Type == "sub").Value);
                var user = await _userRepository.GetUserByIdAsync(userId);
                // Returns if the user is logged in and if the password has expired
                return (true, user?.ExpiredPassword ?? false);
            }
            catch
            {
                return (false, false);
            }
        }

        // Password hashing
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Password verification
        public static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        // JWT token generation
        [SuppressMessage("Security", "S6781",
            Justification = "JWT key is stored in the .env file and not hardcoded in the code, the env name is JWT_KEY, so this is a false positive")]
        public string GenerateJWTToken(User user)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // 'sub' for subject
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(_configuration["JWT_KEY"])
                        ),
                    SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        // Generate and set the JWT token in an HTTP-only cookie
        public string GenerateAndSetHttpOnlyCookie(User user, HttpResponse response)
        {
            var token = GenerateJWTToken(user);

            response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });
            return token;
        }
    }
}