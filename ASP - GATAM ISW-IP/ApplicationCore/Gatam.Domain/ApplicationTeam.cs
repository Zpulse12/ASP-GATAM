using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    class ApplicationTeam
    {
        string Id { get; set; }
        ApplicationUser teamCreator {  get; set; }
        string teamName { get; set; }
        ApplicationUser[] teamUsers { get; set; }
        DateTime createdAt { get; set; }
        bool isDeleted { get; set; }
        public ApplicationTeam(ApplicationUser creator)
        {
            Id = Guid.NewGuid().ToString();
            teamCreator = creator;
            createdAt = DateTime.UtcNow;
            isDeleted = false;
        }
    }
}
