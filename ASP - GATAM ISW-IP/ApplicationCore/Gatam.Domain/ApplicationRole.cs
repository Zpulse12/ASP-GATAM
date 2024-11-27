using Microsoft.AspNetCore.Identity;

namespace Gatam.Domain
{
    public class ApplicationRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
} 