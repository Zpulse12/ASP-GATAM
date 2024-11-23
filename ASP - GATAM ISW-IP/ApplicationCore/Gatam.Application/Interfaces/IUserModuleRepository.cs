using Gatam.Domain;


namespace Gatam.Application.Interfaces;
    public interface IUserModuleRepository : IGenericRepository<UserModule>
    {
       Task<UserModule> FindByIdModuleWithIncludes(string id);
    }   


