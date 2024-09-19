using API.Models;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO);
        bool VerifyPassword(string password, string passwordHash);
    }
    
}