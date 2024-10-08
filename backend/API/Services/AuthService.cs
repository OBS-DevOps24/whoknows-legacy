using API.Data;
using API.Interfaces;
using API.Models;
using API.Models.Dtos;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO)
        {
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

            return (true, "You were successfully registered and can login now");
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

        // JWT token generation
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
                       Encoding.UTF8.GetBytes(_configuration["JWT_Secret"])
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