using Gatam.Domain;

namespace Gatam.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IGenericRepository<ApplicationModule> ModuleRepository { get; }
        public IGenericRepository<UserModule> UserModuleRepository { get; }

        public IQuestionRepository QuestionRepository { get; }
        Task Commit();
    }
}
