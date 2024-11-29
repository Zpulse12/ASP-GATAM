using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Application.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.CQRSTest.ApplicationModule.UserModule
{
    [TestClass]
    public class FindUserModuleIdQueryTest
    {
        private Mock<IUserModuleRepository> _userModuleRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private FindUserModuleIdQueryHandler _queryHandler;

        [TestInitialize]
        public void Setup()
        {
            _userModuleRepositoryMock = new Mock<IUserModuleRepository>();
            _mapperMock = new Mock<IMapper>();

            _queryHandler = new FindUserModuleIdQueryHandler(_userModuleRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidRequest_ReturnsMappedUserModuleDTO()
        {
            var userModuleId = "123";
            var userModule = new Gatam.Domain.UserModule { Id = userModuleId };
            var userModuleDTO = new UserModuleDTO { Id = userModuleId };

            _userModuleRepositoryMock
                .Setup(repo => repo.FindByIdModuleWithIncludes(userModuleId))
                .ReturnsAsync(userModule);

            _mapperMock
                .Setup(mapper => mapper.Map<UserModuleDTO>(userModule))
                .Returns(userModuleDTO);

            var query = new FindUserModuleIdQuery { UserModuleId = userModuleId };

            var result = await _queryHandler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(userModuleDTO.Id, result.Id);

            _userModuleRepositoryMock.Verify(repo => repo.FindByIdModuleWithIncludes(userModuleId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserModuleDTO>(userModule), Times.Once);
        }

    }
}
