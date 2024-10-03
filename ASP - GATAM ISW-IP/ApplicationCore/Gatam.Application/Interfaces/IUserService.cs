using Gatam.Application.CQRS;
using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<ApplicationUser>> GetAllUsers();
    }
}
