using Gatam.Domain;

namespace Gatam.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IModuleRepository ModuleRepository { get; }
        public IUserModuleRepository UserModuleRepository { get; }
        public IGenericRepository<UserAnswer> UserAnwserRepository { get; }
        public IQuestionRepository QuestionRepository { get; }
        Task Commit();
    }
}
