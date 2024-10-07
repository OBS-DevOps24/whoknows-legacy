using API.Models;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO);
        Task<string> SignInAsync(User user);
        Task SignOutAsync(HttpContext httpContext);
        Task<User> AuthenticateUserAsync(string username, string password);
        bool VerifyPassword(string password, string passwordHash);
    }
    
}