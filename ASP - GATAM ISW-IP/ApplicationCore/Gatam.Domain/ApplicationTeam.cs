using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class ApplicationTeam
    {
        public string Id { get; set; }
        public ApplicationUser TeamCreator {  get; set; }
        public string TeamName { get; set; }
        public ApplicationUser[] TeamUsers { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ApplicationTeam(ApplicationUser creator)
        {
            Id = Guid.NewGuid().ToString();
            TeamCreator = creator;
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
