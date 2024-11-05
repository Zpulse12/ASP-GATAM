using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Contexts;
using Gatam.Domain;
using Gatam.Infrastructure.Repositories;

namespace Gatam.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private readonly IGenericRepository<ApplicationModule> _moduleRepository;



        private readonly ApplicationDbContext _context;

        public UnitOfWork(
                            ApplicationDbContext context, 
                            IGenericRepository<ApplicationUser> userRepository, 
                            IGenericRepository<ApplicationModule> moduleRepository)
        {
            _userRepository = userRepository;
            _moduleRepository = moduleRepository;
            _context = context;
        }

        public IGenericRepository<ApplicationUser> UserRepository => _userRepository;
        public IGenericRepository<ApplicationModule> ModuleRepository => _moduleRepository;

        public async Task commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
