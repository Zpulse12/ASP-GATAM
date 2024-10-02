using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        Task commit();
    }
}
