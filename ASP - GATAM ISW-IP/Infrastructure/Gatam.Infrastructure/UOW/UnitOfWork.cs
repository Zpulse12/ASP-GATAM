using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Contexts;
using Gatam.Domain;

namespace Gatam.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<ApplicationModule> _moduleRepository;



        private readonly ApplicationDbContext _context;

        public UnitOfWork(
                            ApplicationDbContext context, 
                            IUserRepository userRepository, 
                            IGenericRepository<ApplicationModule> moduleRepository)
        {
            _userRepository = userRepository;
            _moduleRepository = moduleRepository;
            _context = context;
        }

        public IUserRepository UserRepository => _userRepository;
        public IGenericRepository<ApplicationModule> ModuleRepository => _moduleRepository;

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
