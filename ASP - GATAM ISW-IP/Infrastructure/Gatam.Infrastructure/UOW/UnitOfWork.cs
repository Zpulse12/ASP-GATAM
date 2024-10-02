using Gatam.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepository;

        public UnitOfWork(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IUserRepository UserRepository => _userRepository;



        public async Task commit()
        {
            await Task.CompletedTask;
        }
    }
}
