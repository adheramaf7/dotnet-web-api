using Microsoft.AspNetCore.Identity;

namespace WebApi.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}