using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<ApplicationUser> UserRepository { get; }
        public IGenericRepository<ApplicationModule> ModuleRepository { get; }
        public IGenericRepository<Question> QuestionRepository { get; }
        Task commit();
    }
}
