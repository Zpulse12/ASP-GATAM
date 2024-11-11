using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class Auth0ResponseUsers
    {
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; }
        
       
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }
        
        public string Userid { get; set; }
        public UserMetadata UserMetadata { get; set; }
        public string Username { get; set; }
    }

    public class UserMetadata
    {
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }
    }
}
