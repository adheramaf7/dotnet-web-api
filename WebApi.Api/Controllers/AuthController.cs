using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.DTOs.Request;
using WebApi.Application.DTOs.Request.Auth;
using WebApi.Application.Interfaces.Service;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AuthController(IAuthService authService) : BaseApiController
    {
        private readonly IAuthService authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            var result = await authService.RegisterAsync(request);

            return Success(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var result = await authService.LoginAsync(request);

            return Success(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenRequest request)
        {
            var accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var result = await authService.RefreshTokenAsync(request.RefreshToken, accessToken);

            return Success(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            await authService.RevokeTokenAsync(accessToken);

            return Success<object>(null, "Logout Success");
        }
    }
}