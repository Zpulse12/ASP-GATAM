using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public ApplicationUserRoles _role { get; set; }

        public required string Id { get; set; }
    }
}
