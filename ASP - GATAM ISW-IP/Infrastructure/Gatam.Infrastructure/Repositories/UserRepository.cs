using Gatam.Infrastructure.Contexts;
using Gatam.Domain;

namespace Gatam.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }
    }
}
