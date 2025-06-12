using System.ComponentModel;

namespace WebApi.Domain.Enums
{
    public enum UserRole
    {
        [Description("Super Administrator")] SuperAdmin,
        [Description("Administrator")] Admin,
    }
}