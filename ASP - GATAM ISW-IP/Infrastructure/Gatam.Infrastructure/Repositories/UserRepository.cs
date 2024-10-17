using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Contexts;
using Gatam.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }
        public async Task<ApplicationUser?> FindByName(string name)
        {
            return await FindByProperty("UserName", name);
        }
        public async Task<ApplicationUser?> FindByEmail(string email)
        {
            return await FindByProperty("Email", email);
        }
    }
}
