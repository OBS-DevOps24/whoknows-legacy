using API.Data;
using API.Interfaces;
using API.Models;


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

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}