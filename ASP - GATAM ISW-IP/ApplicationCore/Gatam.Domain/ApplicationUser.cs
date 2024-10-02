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
        public ApplicationUserRoles _role { get; set; }

        public ApplicationUser(ApplicationUserRoles role = ApplicationUserRoles.STUDENT) 
        { 
            this._role = role;
        }


    }

}
