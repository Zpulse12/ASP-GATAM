using Gatam.Application.Interfaces;
using Gatam.Authentication.Data;
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
    }
}
