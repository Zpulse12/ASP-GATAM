using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gatam.Infrastructure.Repositories
{
    public class ModuleRepository : GenericRepository<ApplicationModule>, IModuleRepository
    {
        private readonly ApplicationDbContext _context;

        public ModuleRepository(ApplicationDbContext context) : base(context) {
            _context = context;
        }

        public async Task<ApplicationModule> GetModuleWithQuestionsAndAnswersAsync(string moduleId)
        {
            return await _context.Modules
                .Include(m => m.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(m => m.Id == moduleId);
        }

        

        public async Task<ApplicationModule> FindByIdWithQuestions(string moduleId)
        {
            return await _context.Set<ApplicationModule>()
                .Include(m => m.Questions) // Haal de vragen op
                                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(m => m.Id == moduleId);
        }

        public async Task<ApplicationModule> FindByIdModuleWithIncludes(string id)
        {
            return await _context.Modules
                .Include(m => m.Questions) 
                    .ThenInclude(q => q.Answers) 
                        .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
