using Gatam.Domain;
using Gatam.Infrastructure.Contexts;

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
