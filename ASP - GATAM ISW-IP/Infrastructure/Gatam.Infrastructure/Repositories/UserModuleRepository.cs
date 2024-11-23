using Auth0.ManagementApi.Models;
using Azure.Core;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
                            .Include(um => um.Module)
                                .ThenInclude(m => m.Questions)
                                    .ThenInclude(q => q.Answers)
                                        .ThenInclude(x => x.GivenUserAnswers)
                                            .FirstOrDefaultAsync(um => um.Id == id);
        }

    }
}
