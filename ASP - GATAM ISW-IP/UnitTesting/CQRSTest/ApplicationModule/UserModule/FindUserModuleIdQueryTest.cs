using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationModule.UserModule
{
    [TestClass]
    public class FindUserModuleIdQueryTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMapper> _mapperMock;
        private FindUserModuleIdQueryHandler _queryHandler;

        [TestInitialize]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _queryHandler = new FindUserModuleIdQueryHandler(_uowMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidRequest_ReturnsMappedUserModuleDTO()
        {
            var userModuleId = "123";
            var userModule = new Gatam.Domain.UserModule { Id = userModuleId };
            var userModuleDTO = new UserModuleDTO { Id = userModuleId };

            _uowMock
                .Setup(repo => repo.UserModuleRepository.FindByIdModuleWithIncludes(userModuleId))
                .ReturnsAsync(userModule);

            _mapperMock
                .Setup(mapper => mapper.Map<UserModuleDTO>(userModule))
                .Returns(userModuleDTO);

            var query = new FindUserModuleIdQuery { UserModuleId = userModuleId };

            var result = await _queryHandler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(userModuleDTO.Id, result.Id);

            _uowMock.Verify(repo => repo.UserModuleRepository.FindByIdModuleWithIncludes(userModuleId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserModuleDTO>(userModule), Times.Once);
        }

    }
}
