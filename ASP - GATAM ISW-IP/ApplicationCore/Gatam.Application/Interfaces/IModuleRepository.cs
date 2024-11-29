using Gatam.Domain;

namespace Gatam.Application.Interfaces;
    public interface IModuleRepository : IGenericRepository<ApplicationModule>
{
    
        Task<ApplicationModule> FindByIdWithQuestions(string moduleId);
        
}