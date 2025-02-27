﻿using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

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

        
    }
}
