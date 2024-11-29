using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Gatam.Infrastructure.Repositories
{
    public class UserModuleRepository : GenericRepository<UserModule>, IUserModuleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserModuleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<UserModule> FindByIdModuleWithIncludes(string id)
        {
            return await _context.UserModules
        .Include(x => x.Module)
            .ThenInclude(x => x.Questions)
            .ThenInclude(x => x.Answers)
        .Include(x => x.UserGivenAnswers)
        .Include(x => x.UserQuestions)
        .FirstOrDefaultAsync(um => um.Id == id);
        }

    }
}
