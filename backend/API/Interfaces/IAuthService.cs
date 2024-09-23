using API.Models;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO);
        Task SignInAsync(HttpContext httpContext, User user);
        Task SignOutAsync(HttpContext httpContext);
        Task<User> AuthenticateUserAsync(string username, string password);
        bool VerifyPassword(string password, string passwordHash);
    }
    
}