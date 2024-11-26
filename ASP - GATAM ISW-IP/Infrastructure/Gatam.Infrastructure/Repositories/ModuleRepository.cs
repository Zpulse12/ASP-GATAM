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
        public async Task<ApplicationModule> FindByIdWithQuestions(string moduleId)
        {
            return await _context.Set<ApplicationModule>()
                .Include(m => m.Questions) 
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(m => m.Id == moduleId);
        }

        public async Task UpdateModuleWithQuestions(ApplicationModule module)
        {
            var existingModule = await _context.Modules
                .Include(m => m.Questions) 
                .FirstOrDefaultAsync(m => m.Id == module.Id);

            if (existingModule == null)
            {
                throw new KeyNotFoundException($"Module met ID {module.Id} niet gevonden.");
            }

            _context.Entry(existingModule).CurrentValues.SetValues(module);

            foreach (var question in module.Questions)
            {
                var existingQuestion = existingModule.Questions.FirstOrDefault(q => q.Id == question.Id);

                if (existingQuestion != null)
                {
                    _context.Entry(existingQuestion).CurrentValues.SetValues(question);
                }
                else
                {
                    existingModule.Questions.Add(question); 
                }
            }

            foreach (var existingQuestion in existingModule.Questions.ToList())
            {
                if (!module.Questions.Any(q => q.Id == existingQuestion.Id))
                {
                    _context.Questions.Remove(existingQuestion);
                }
            }
        }
    }
}
