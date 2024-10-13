using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Repositories
{
    public class TeamRepository : GenericRepository<ApplicationTeam>
    {
        public TeamRepository(ApplicationDbContext context) : base(context) { }
    }
    
}
