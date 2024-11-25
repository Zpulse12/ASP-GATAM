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



        private readonly ApplicationDbContext _context;

        public UnitOfWork(IUserQuestionRepository userQuestionRepository,
                            ApplicationDbContext context, 
                            IUserRepository userRepository,
                            IModuleRepository moduleRepository,
                            IQuestionRepository questionRepository)
        {
            _userRepository = userRepository;
            _moduleRepository = moduleRepository;
            _questionRepository = questionRepository;
            _userQuestionRepository = userQuestionRepository;
            _context = context;
        }

        public IUserRepository UserRepository => _userRepository;
        public IModuleRepository ModuleRepository => _moduleRepository;

        public IQuestionRepository QuestionRepository => _questionRepository;
        public IUserQuestionRepository UserQuestionRepository => _userQuestionRepository;
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
