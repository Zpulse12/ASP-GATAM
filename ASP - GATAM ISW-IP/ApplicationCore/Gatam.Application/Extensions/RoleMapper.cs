namespace Gatam.Application.Extensions
{
    public static class RoleMapper
    {
        public static readonly Dictionary<string, string> Roles = new Dictionary<string, string>
        {
            { "BEHEERDER", "BEHEERDER" },
            { "BEGELEIDER", "rol_8cLkJwwd2u2itnu3" },
            { "VOLGER", "rol_2SgoYL1AoK9tXYXW" },
            { "MAKER", "rol_tj8keS38380ZU4NR" }
        };



        public static string[] GetRoleValues(params string[] roleNames)
        {
            return Roles
                .Where(role => roleNames.Contains(role.Key))
                .Select(role => role.Value)
                .ToArray();
        }

    }
}
