using Microsoft.AspNetCore.Identity;
using WebApi.Application.DTOs.Request;
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

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                Token = token
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

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token,
            };
        }
    }
}