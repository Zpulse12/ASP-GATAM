using Gatam.Application.Extensions;
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
        public required string Nickname { get; set; }
        public required string Email { get; set; }
        public required IEnumerable<string> RolesIds { get; set; } 

        public required string Id { get; set; }
        public bool IsActive { get; set; }
        public string Picture { get; set; }
        
    }
}
