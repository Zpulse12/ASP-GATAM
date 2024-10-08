using AutoMapper;
using Gatam.Application.CQRS;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    [TestClass]
    public class GetAllUsersQueryHandlerTests
    {
        private Mock<IUnitOfWork>? unitOfWorkMock;
        private Mock<IGenericRepository<ApplicationUser>>? userRepositoryMock;
        private Mock<IMapper>? mapperMock;
        private GetAllUsersQueryHandler? handler;

        [TestInitialize]
        public void Setup()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            userRepositoryMock = new Mock<IGenericRepository<ApplicationUser>>();
            mapperMock = new Mock<IMapper>();

            unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

            handler = new GetAllUsersQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnUserDTOs_WhenUsersAreFound()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "user1", Email = "user1@example.com", PhoneNumber = "09583636", AccessFailedCount =21, _role = ApplicationUserRoles.ADMIN }, 
                new ApplicationUser { UserName = "user2", Email = "user2@example.com", PhoneNumber = "0488356346", AccessFailedCount = 11}  
            };

            var userDTOs = new List<UserDTO>
            {
                new UserDTO { Username = "user1", Email = "user1@example.com", _role = ApplicationUserRoles.ADMIN }, 
                new UserDTO { Username = "user2", Email = "user2@example.com" }  
            };

            userRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(users);

            mapperMock.Setup(m => m.Map<IEnumerable<UserDTO>>(It.IsAny<IEnumerable<ApplicationUser>>()))
                .Returns(userDTOs);

            var query = new GetAllUsersQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(userDTOs.Count, result.Count());
            userRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<UserDTO>>(users), Times.Once);
        }

    }
}
