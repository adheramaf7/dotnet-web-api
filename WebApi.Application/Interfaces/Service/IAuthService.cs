using WebApi.Application.DTOs.Request;
using WebApi.Application.DTOs.Response;

namespace WebApi.Application.Interfaces.Service
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}