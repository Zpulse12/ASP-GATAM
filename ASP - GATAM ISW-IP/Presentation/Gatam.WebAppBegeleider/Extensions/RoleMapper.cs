using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Gatam.WebAppBegeleider.Extensions
{

    public class RoleInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public enum CustomRoles
    {
        BEHEERDER,
        BEGELEIDER,
        VOLGER,
        MAKER
    }
    public static class RoleMapper
    {
        public static readonly ImmutableDictionary<CustomRoles, RoleInfo> Roles = ImmutableDictionary.CreateRange(new[]
        {
        new KeyValuePair<CustomRoles, RoleInfo>(CustomRoles.BEHEERDER, new RoleInfo() { Name = "BEHEERDER", Id = "rol_3BsJHRFokYkbjr5O" }),
        new KeyValuePair<CustomRoles, RoleInfo>(CustomRoles.BEGELEIDER, new RoleInfo() { Name = "BEGELEIDER", Id = "rol_8cLkJwwd2u2itnu3" }),
        new KeyValuePair<CustomRoles, RoleInfo>(CustomRoles.VOLGER, new RoleInfo() { Name = "VOLGER", Id = "rol_2SgoYL1AoK9tXYXW" }),
        new KeyValuePair<CustomRoles, RoleInfo>(CustomRoles.MAKER, new RoleInfo() { Name = "MAKER", Id = "rol_tj8keS38380ZU4NR" })
        });

        public static RoleInfo[] GetRoleValues(params CustomRoles[] roleNames)
        {
            if (roleNames == null) throw new ArgumentNullException(nameof(roleNames));
            return roleNames
                .Select(role => Roles.TryGetValue(role, out var value)
                    ? value
                    : throw new ArgumentException($"Role {role} not found"))
                .ToArray();
        }
        public static string[] GetListOfRoleIds(params CustomRoles[] roleNames)
        {
            if (roleNames == null) throw new ArgumentNullException(nameof(roleNames));
            return roleNames
                .Select(role => Roles.TryGetValue(role, out var value)
                    ? value.Id
                    : throw new ArgumentException($"Role {role} not found"))
                .ToArray();
        }

        public static string[] GetListOfRoleNames(params CustomRoles[] roleNames)
        {
            if (roleNames == null) throw new ArgumentNullException(nameof(roleNames));
            return roleNames
                .Select(role => Roles.TryGetValue(role, out var value)
                    ? value.Name
                    : throw new ArgumentException($"Role {role} not found"))
                .ToArray();
        }

        public static List<CustomRoles> GetKeysBasedOnValues(params string[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            return values
                .Select(value => Roles.FirstOrDefault(role => role.Value.Name == value || role.Value.Id == value).Key)
                .Where(key => key != default)
                .ToList();
        }
    }
}
