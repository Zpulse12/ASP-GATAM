using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class TeamInvitation
    {
        public string Id { get; set; }
        public required string ApplicationTeamId { get; set; }
        public ApplicationTeam applicationTeam { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser applicationUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public bool isAccepted { get; set; }
        public TeamInvitation() { 
            
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            ResponseDateTime = DateTime.UtcNow;
            isAccepted = false;
        }
    }
}
