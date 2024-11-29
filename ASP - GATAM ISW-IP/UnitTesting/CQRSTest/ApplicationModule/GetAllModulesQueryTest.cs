using AutoMapper;
using Gatam.Application.CQRS.Module;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task Handle_ShouldReturnAllModules_WhenModulesExist()
        {
            var modules = new List<Gatam.Domain.ApplicationModule>
             {
                 new Gatam.Domain.ApplicationModule { Title = "Module 1", Category = "Category 1" },
                 new Gatam.Domain.ApplicationModule { Title = "Module 2", Category = "Category 2" }
             };

            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Gatam.Domain.ApplicationModule, object>>>()))
                .ReturnsAsync(modules);

            _mapperMock.Setup(m => m.Map<IEnumerable<Gatam.Domain.ApplicationModule>>(It.IsAny<IEnumerable<Gatam.Domain.ApplicationModule>>()))
                .Returns((IEnumerable<Gatam.Domain.ApplicationModule> source) => source);

            var query = new GetAllModulesQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(2, result.Count());
            CollectionAssert.AreEqual(modules, result.ToList());
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
