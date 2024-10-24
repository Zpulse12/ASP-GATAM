using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS
{
    public class TeamInvitationDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ApplicationTeamId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public bool IsAccepted { get; set; }
    }
}
