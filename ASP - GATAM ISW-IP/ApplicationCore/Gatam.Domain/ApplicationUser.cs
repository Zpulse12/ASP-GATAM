using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        public bool IsActive { get; set; }

        [JsonIgnore]
        public ICollection<ApplicationTeam> OwnedApplicationTeams { get; } = new List<ApplicationTeam>();
        [JsonIgnore]
        public ICollection<TeamInvitation> InvitationsRequests { get; } = new List<TeamInvitation>();
    }
}
