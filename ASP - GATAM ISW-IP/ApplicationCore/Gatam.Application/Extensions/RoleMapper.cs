using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions
{
    public static class RoleMapper
    {
        public const string Admin = "BEHEERDER";
        public const string Begeleider = "BEGELEIDER";
        public const string Volger = "VOLGER";
        public const string ContentMaker = "MAKER";
        public static List<string> GetAllRoles()
        {
            return new List<string> { Admin, Begeleider, Volger, ContentMaker };
        }
    }
}
