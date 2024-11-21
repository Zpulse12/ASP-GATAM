using Gatam.Domain;

namespace Gatam.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IGenericRepository<ApplicationModule> ModuleRepository { get; }
        public IQuestionRepository QuestionRepository { get; }
        public IGenericRepository<UserModuleQuestionSetting> UserModuleQuestionSettingRepository { get; }
        Task Commit();
    }
}
