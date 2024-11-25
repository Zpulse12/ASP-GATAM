using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Contexts;
using Gatam.Domain;
using Gatam.Infrastructure.Repositories;

namespace Gatam.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserQuestionRepository _userQuestionRepository;
        private readonly IUserModuleRepository _userModuleRepository;
        private readonly IGenericRepository<UserAnswer> _userAnwserRepository;

        private readonly ApplicationDbContext _context;

        public UnitOfWork(IUserQuestionRepository userQuestionRepository,
                            ApplicationDbContext context, 
                            IUserRepository userRepository,
                            IModuleRepository moduleRepository,
                            IQuestionRepository questionRepository,
                             IUserModuleRepository userModuleRepository,
                            IGenericRepository<UserAnswer> userAnwserRepository)

        {
            _userRepository = userRepository;
            _moduleRepository = moduleRepository;
            _questionRepository = questionRepository;
            _userQuestionRepository = userQuestionRepository;
            _context = context;
            _userModuleRepository = userModuleRepository;
            _userAnwserRepository = userAnwserRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IModuleRepository ModuleRepository => _moduleRepository;
        public IUserModuleRepository UserModuleRepository => _userModuleRepository;
        public IQuestionRepository QuestionRepository => _questionRepository;
        public IUserQuestionRepository UserQuestionRepository => _userQuestionRepository;
        public IGenericRepository<UserAnswer> UserAnwserRepository => _userAnwserRepository;
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
