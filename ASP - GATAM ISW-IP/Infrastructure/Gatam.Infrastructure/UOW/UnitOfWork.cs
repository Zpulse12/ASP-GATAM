using Gatam.Application.Interfaces;
using Gatam.Authentication.Data;
using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context, IGenericRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public IGenericRepository<ApplicationUser> UserRepository => _userRepository;

        public async Task commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
