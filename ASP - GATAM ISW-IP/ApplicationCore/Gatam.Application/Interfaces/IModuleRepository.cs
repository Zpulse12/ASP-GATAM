using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Interfaces
{
    public interface IModuleRepository : IGenericRepository<ApplicationModule>
    {
        Task<ApplicationModule> GetModuleWithQuestionsAndAnswersAsync(string moduleId);

        Task<ApplicationModule> FindByIdWithQuestions(string moduleId);
    }
}