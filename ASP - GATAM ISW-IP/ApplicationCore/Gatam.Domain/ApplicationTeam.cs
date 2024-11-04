using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class ApplicationTeam
    {
        public string Id { get; set; }
        public required string TeamCreatorId { get; set; }
        public ApplicationUser TeamCreator {  get; set; }
        public string TeamName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<TeamInvitation> TeamInvitations { get; set; } = new List<TeamInvitation>();
        public ApplicationTeam() {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
            TeamName = Guid.NewGuid().ToString();
        }

    }
}
