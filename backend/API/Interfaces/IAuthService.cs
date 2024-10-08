using API.Models;
using API.Models.Dtos;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDTO);
        Task<(bool Success, string Message)> LoginAsync(LoginDTO loginDTO, HttpResponse response);
        Task<(bool Success, string Message)> LogoutAsync(string token, HttpResponse response);
    }
    
}