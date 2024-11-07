using Microsoft.AspNetCore.Identity;


namespace Gatam.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
    }
}
