using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gatam.Domain
{

    public enum ApplicationUserRoles
    {
        ADMIN,
        MENTOR,
        STUDENT
    }
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUserRoles _role { get; set; }
        public ICollection<ApplicationTeam> OwnedApplicationTeams { get; } = new List<ApplicationTeam>();

    }
}
