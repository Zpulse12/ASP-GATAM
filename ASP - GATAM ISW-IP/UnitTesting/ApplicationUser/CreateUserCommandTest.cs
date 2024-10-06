using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ApplicationUser
{
    [TestClass]
    internal class CreateUserCommandTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<UserRepository> _userRepositoryMock;

        [TestInitialize]
        public void Setup()
        {

        }
    }
}
