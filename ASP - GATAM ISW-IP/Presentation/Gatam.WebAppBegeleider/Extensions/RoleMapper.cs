using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Gatam.WebAppBegeleider.Extensions
{
    public enum CustomRoles
    {
        BEHEERDER,
        BEGELEIDER,
        VOLGER,
        MAKER
    }
    public static class RoleMapper
    {
        public static readonly ImmutableDictionary<CustomRoles, string> Roles = ImmutableDictionary.CreateRange(new[]
        {
        new KeyValuePair<CustomRoles, string>(CustomRoles.BEHEERDER, "BEHEERDER"),
        new KeyValuePair<CustomRoles, string>(CustomRoles.BEGELEIDER, "BEGELEIDER"),
        new KeyValuePair<CustomRoles, string>(CustomRoles.VOLGER, "VOLGER"),
        new KeyValuePair<CustomRoles, string>(CustomRoles.MAKER, "MAKER")
        });

        public static string[] GetRoleValues(params CustomRoles[] roleNames)
        {
            if (roleNames == null) throw new ArgumentNullException(nameof(roleNames));
            return roleNames
                .Select(role => Roles.TryGetValue(role, out var value)
                    ? value
                    : throw new ArgumentException($"Role {role} not found"))
                .ToArray();
        }

        public static List<CustomRoles> GetKeysBasedOnValues(params string[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            return values
                .Select(value => Roles.FirstOrDefault(role => role.Value == value).Key)
                .Where(key => key != default)
                .ToList();
        }
    }
}
