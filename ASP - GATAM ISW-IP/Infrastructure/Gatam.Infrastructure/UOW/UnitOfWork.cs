using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Contexts;
using Gatam.Domain;
using Gatam.Infrastructure.Repositories;

namespace Gatam.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<ApplicationModule> _moduleRepository;
        private readonly IQuestionRepository _questionRepository;



        private readonly ApplicationDbContext _context;

        public UnitOfWork(
                            ApplicationDbContext context, 
                            IUserRepository userRepository, 
                            IGenericRepository<ApplicationModule> moduleRepository,
                            IQuestionRepository genericRepository)
        {
            _userRepository = userRepository;
            _moduleRepository = moduleRepository;
            _questionRepository = genericRepository;
            _context = context;
        }

        public IUserRepository UserRepository => _userRepository;
        public IGenericRepository<ApplicationModule> ModuleRepository => _moduleRepository;

        public IQuestionRepository QuestionRepository => _questionRepository;

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
