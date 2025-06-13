using System.Dynamic;

namespace WebApi.Application.DTOs.Response
{
    public class AuthResponse
    {

        public UserProfileResponse User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

    }
}