using Microsoft.AspNetCore.Identity;
using WebApi.Application.DTOs.Request;
using WebApi.Application.DTOs.Request.Auth;
using WebApi.Application.DTOs.Response;
using WebApi.Application.Interfaces.Service;
using WebApi.Domain.Enums;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenGenerator tokenGenerator) : IAuthService
    {

        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly SignInManager<ApplicationUser> signInManager = signInManager;
        private readonly JwtTokenGenerator tokenGenerator = tokenGenerator;

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email) ?? throw new UnauthorizedAccessException("Invalid credentials.");

            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded == false)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenGenerator.GenerateToken(user, roles);
            var refreshToken = tokenGenerator.GenerateRefreshToken();

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                Token = token,
                RefreshToken = refreshToken,
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = tokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
            var email = principal?.Identity?.Name;

            if (email is null)
                throw new Exception("Invalid access token");

            var user = await userManager.FindByEmailAsync(email);

            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            var roles = await userManager.GetRolesAsync(user);

            var newAccessToken = tokenGenerator.GenerateToken(user, roles);
            var newRefreshToken = tokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await userManager.UpdateAsync(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded == false)
            {
                throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());

            var token = tokenGenerator.GenerateToken(user, [UserRole.Admin.ToString()]);
            var newRefreshToken = tokenGenerator.GenerateRefreshToken();

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token,
                RefreshToken = newRefreshToken,
            };
        }
    }
}