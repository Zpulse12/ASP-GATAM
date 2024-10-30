using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions
{
    public static class RoleMapper
    {
        public const string Beheerder = "BEHEERDER";
        public const string Begeleider = "BEGELEIDER";
        public const string Volger = "VOLGER";
        public const string Content_Maker = "MAKER";

        public static List<string> GetAllRoles()
        {
            return new List<string> { Beheerder, Begeleider, Volger, Content_Maker };
        }

        
    }
}
