using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApi.Application.Common;
using WebApi.Application.DTOs.Request;
using WebApi.Application.DTOs.Request.Auth;
using WebApi.Application.DTOs.Response;
using WebApi.Application.Exceptions;
using WebApi.Application.Interfaces.Service;
using WebApi.Domain.Enums;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenGenerator tokenGenerator, IMapper mapper) : IAuthService
    {

        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly SignInManager<ApplicationUser> signInManager = signInManager;
        private readonly JwtTokenGenerator tokenGenerator = tokenGenerator;
        private readonly IMapper mapper = mapper;

        public async Task ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User");

            var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded == false)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();

                throw new AppException("Change Password Failed", errors);
            }
        }

        public async Task<UserProfileResponse> GetUserProfileAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User");

            var roles = await userManager.GetRolesAsync(user);

            var profile = mapper.Map<UserProfileResponse>(user);
            profile.Roles = roles;

            return profile;
        }

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

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await userManager.UpdateAsync(user);

            var userProfile = mapper.Map<UserProfileResponse>(user);
            userProfile.Roles = roles;

            return new AuthResponse
            {
                User = userProfile,
                Token = token,
                RefreshToken = refreshToken,
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken, string accessToken)
        {
            var principal = tokenGenerator.GetPrincipalFromExpiredToken(accessToken);

            var email = (principal?.Identity?.Name) ?? throw new AppException("Invalid access token");
            var user = await userManager.FindByEmailAsync(email);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new AppException("Invalid refresh token");

            var roles = await userManager.GetRolesAsync(user);

            var newAccessToken = tokenGenerator.GenerateToken(user, roles);
            var newRefreshToken = tokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await userManager.UpdateAsync(user);

            var userProfile = mapper.Map<UserProfileResponse>(user);
            userProfile.Roles = roles;

            return new AuthResponse
            {
                User = userProfile,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var newRefreshToken = tokenGenerator.GenerateRefreshToken();

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded == false)
            {
                throw new AppException(string.Join(", ", result.Errors.Select(e => e.Description)), [.. result.Errors.Select(e => e.Description)]);
            }

            await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());

            var roles = new string[] {
                UserRole.Admin.ToString()
            };

            var token = tokenGenerator.GenerateToken(user, roles);

            var userProfile = mapper.Map<UserProfileResponse>(user);
            userProfile.Roles = roles;

            return new AuthResponse
            {
                User = userProfile,
                Token = token,
                RefreshToken = newRefreshToken,
            };
        }

        public async Task RevokeTokenAsync(string accessToken)
        {
            var principal = tokenGenerator.GetPrincipalFromExpiredToken(accessToken);

            var email = (principal?.Identity?.Name) ?? throw new UnauthorizedAccessException("Invalid token.");

            var user = await userManager.FindByEmailAsync(email) ?? throw new UnauthorizedAccessException("Invalid token.");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await userManager.UpdateAsync(user);
        }

        public async Task<UserProfileResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var user = await userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User");

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.UserName = request.Email;

            var roles = await userManager.GetRolesAsync(user);

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded == false)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();

                throw new AppException("Update profile failed", errors);
            }

            var userProfile = mapper.Map<UserProfileResponse>(user);
            userProfile.Roles = roles;

            return userProfile;
        }

    }
}