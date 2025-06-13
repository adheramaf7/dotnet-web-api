using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Application.DTOs.Request.Auth
{
    public class UpdateProfileRequest
    {

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}