using AutoMapper;
using Gatam.Application.CQRS.Module;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationModule
{
    [TestClass]
    public class GetAllModulesQueryTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private GetAllModulesQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllModulesQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnEmptyList_WhenNoModulesExist()
        {
            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Gatam.Domain.ApplicationModule, object>>>()))
                .ReturnsAsync(new List<Gatam.Domain.ApplicationModule>());

            var query = new GetAllModulesQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(0, result.Count());
        }
    }
}
