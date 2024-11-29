using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Repositories
{
    public class UserAnwserRepository : GenericRepository<UserAnswer>
    {
        private readonly ApplicationDbContext _context;

        public UserAnwserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
