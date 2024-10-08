using API.Models;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO);
        Task<string> SignInAsync(User user);
        Task SignOutAsync(string token);
        Task<User> AuthenticateUserAsync(string username, string password);
        bool VerifyPassword(string password, string passwordHash);
    }
    
}