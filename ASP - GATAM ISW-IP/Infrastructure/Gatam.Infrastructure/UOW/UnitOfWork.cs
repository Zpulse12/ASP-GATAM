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
        private readonly IGenericRepository<UserModule> _userModuleRepository;




        private readonly ApplicationDbContext _context;

        public UnitOfWork(
                            ApplicationDbContext context, 
                            IUserRepository userRepository, 
                            IGenericRepository<ApplicationModule> moduleRepository,
                            IQuestionRepository questionRepository,
                            IGenericRepository<UserModule> userModuleRepository

                            )
        {
            _userRepository = userRepository;
            _moduleRepository = moduleRepository;
            _questionRepository = questionRepository;
            _context = context;
            _userModuleRepository = userModuleRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IGenericRepository<ApplicationModule> ModuleRepository => _moduleRepository;
        public IGenericRepository<UserModule> UserModuleRepository => _userModuleRepository;
        public IQuestionRepository QuestionRepository => _questionRepository;

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
