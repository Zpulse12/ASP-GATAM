using Gatam.Domain;

namespace Gatam.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<ApplicationUser> UserRepository { get; }
        public IGenericRepository<ApplicationModule> ModuleRepository { get; }
        Task Commit();
    }
}
