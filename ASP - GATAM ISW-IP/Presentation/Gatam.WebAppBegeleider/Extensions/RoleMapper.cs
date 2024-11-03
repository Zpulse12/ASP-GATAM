using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.WebAppBegeleider.Extensions
{
    public static class RoleMapper
    {
        public const string Admin = "ADMIN";
        public const string Begeleider = "MENTOR";
        public const string Volger = "STUDENT";
        public static List<string> GetAllRoles()
        {
            return new List<string> { Admin, Begeleider, Volger };
        }
    }
}
