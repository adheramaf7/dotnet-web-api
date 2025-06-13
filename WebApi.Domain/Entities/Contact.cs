using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Domain.Common;

namespace WebApi.Domain.Entities
{
    public class Contact : BaseEntity
    {

        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty!;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}