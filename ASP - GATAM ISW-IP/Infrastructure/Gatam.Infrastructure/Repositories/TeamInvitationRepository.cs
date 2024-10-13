using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Repositories
{
    public class TeamInvitationRepository : GenericRepository<TeamInvitation>
    {
        public TeamInvitationRepository(ApplicationDbContext context) : base(context) { }
    }
}
