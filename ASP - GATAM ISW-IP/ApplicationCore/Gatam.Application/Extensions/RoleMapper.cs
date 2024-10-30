using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions
{
    public static class RoleMapper
    {
        public static readonly Dictionary<string, string> Roles = new Dictionary<string, string>
        {
            { "BEHEERDER", "rol_3BsJHRFokYkbjr5O" },
            { "BEGELEIDER", "rol_8cLkJwwd2u2itnu3" },
            { "VOLGER", "rol_2SgoYL1AoK9tXYXW" },
            { "MAKER", "rol_tj8keS38380ZU4NR" }
        };

       

        public static List<string> GetAllRoles()
        {
            return Roles.Keys.ToList();
        }

        public static string? GetRoleId(string roleName)
        {
            return Roles.TryGetValue(roleName, out var roleId) ? roleId : null;
        }

    }
}
