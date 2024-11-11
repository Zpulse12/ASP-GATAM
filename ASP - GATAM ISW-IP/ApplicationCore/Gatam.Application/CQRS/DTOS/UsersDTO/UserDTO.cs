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
        public  string Id { get; set; }
        public  string Name { get; set; }
        public  string Username { get; set; }
        public string Surname { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public  IEnumerable<string>? RolesIds { get; set; }
        public bool IsActive { get; set; }
        public string? Picture { get; set; }
        
    }
}
