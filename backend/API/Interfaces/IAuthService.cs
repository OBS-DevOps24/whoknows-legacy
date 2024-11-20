using API.Models;
using API.Models.Dtos;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO, HttpResponse response);
        Task<(bool Success, string Message)> LoginAsync(LoginDTO loginDTO, HttpResponse response);
        Task<(bool Success, string Message)> LogoutAsync(string token, HttpResponse response);
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDTO, HttpResponse response);
        Task<(bool IsLoggedIn, bool ExpiredPassword)> CheckLoginStatusAsync(string token);
    }
}