namespace WebApi.Application.DTOs.Response
{
    public class AuthResponse
    {

        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

    }
}