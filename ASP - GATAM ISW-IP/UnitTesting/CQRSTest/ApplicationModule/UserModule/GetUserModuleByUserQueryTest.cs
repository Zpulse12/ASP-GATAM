using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationModule.UserModule
{
    [TestClass]
    public class GetUserModuleByUserQueryTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMapper> _mapperMock;
        private GetUserModuleByUserQueryHandler _queryHandler;

        [TestInitialize]
        public void Setup()
        {
            // Mock dependencies
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            // Instantiate the handler
            _queryHandler = new GetUserModuleByUserQueryHandler(_uowMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidRequest_ReturnsMappedUserModules()
        {
            // Arrange
            var userId = "valid-user-id";
            var userModules = new List<Gatam.Domain.UserModule>
            {
                new Gatam.Domain.UserModule { Id = "module1" },
                new Gatam.Domain.UserModule { Id = "module2" }
            };
            var user = new Gatam.Domain.ApplicationUser { Id = userId, UserModules = userModules };

            var userModuleDTOs = new List<UserModuleDTO>
            {
                new UserModuleDTO { Id = "module1" },
                new UserModuleDTO { Id = "module2"}
            };

            _uowMock
                .Setup(repo => repo.UserRepository.GetUserWithModules(userId))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(mapper => mapper.Map<List<UserModuleDTO>>(userModules))
                .Returns(userModuleDTOs);

            var query = new GetUserModuleByUserQuery() { UserId = userId };

            var result = await _queryHandler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(userModuleDTOs.Count, result.Count);
            CollectionAssert.AreEquivalent(userModuleDTOs, result);

            _uowMock.Verify(repo => repo.UserRepository.GetUserWithModules(userId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<UserModuleDTO>>(userModules), Times.Once);
        }
    }
}
