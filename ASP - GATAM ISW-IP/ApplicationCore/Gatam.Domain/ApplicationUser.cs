using Microsoft.AspNetCore.Identity;

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
        public ApplicationUserRoles Role { get; set; }
        public bool IsActive { get; set; }

    }

}
