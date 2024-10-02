using Gatam.Application.Interfaces;
using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly List<User> _users = new List<User>
        {
            new User { Name = "John Doe", Email = "john.doe@example.com", Password = "hashedpassword1" },
            new User { Name = "Jane Doe", Email = "jane.doe@example.com", Password = "hashedpassword2" }
        };

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await Task.FromResult(_users);
        }
    }
}
