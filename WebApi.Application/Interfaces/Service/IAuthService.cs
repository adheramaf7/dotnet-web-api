using WebApi.Application.DTOs.Request;
using WebApi.Application.DTOs.Request.Auth;
using WebApi.Application.DTOs.Response;

namespace WebApi.Application.Interfaces.Service
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken, string accessToken);
        Task RevokeTokenAsync(string accessToken);
        Task ChangePasswordAsync(string userId, ChangePasswordRequest request);
        Task<UserProfileResponse> GetUserProfileAsync(string userId);
        Task<UserProfileResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    }
}