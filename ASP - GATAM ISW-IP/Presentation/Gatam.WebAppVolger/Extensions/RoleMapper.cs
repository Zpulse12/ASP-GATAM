﻿namespace Gatam.WebAppVolger.Extensions
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
        public static string[] GetRoleValues(params string[] roleNames)
        {
            return Roles
                .Where(role => roleNames.Contains(role.Key))
                .Select(role => role.Key)
                .ToArray();
        }

        public static List<string> GetKeysBasedOnValues(params string[] values)
        {
            return Roles.Where(role => values.Contains(role.Value)).Select(role => role.Key).ToList();
        }

    }
}
