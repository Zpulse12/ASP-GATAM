using AutoMapper;
using Gatam.Application.CQRS;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTesting.QuerCQRSTestyTest.ApplicationTeam
{
    [TestClass]
    internal class GetAllTeamQueryTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IMapper> mockMapper;
        private IRequestHandler<GetAllTeamsQuery, IEnumerable<Gatam.Domain.ApplicationTeam>> handler;

        private ApplicationUser GLOBALTESTUSER = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "admin",
            Email = "admin@app.com"
        };

        private Gatam.Domain.ApplicationTeam GLOBALTESTTEAM = new Gatam.Domain.ApplicationTeam()
        {
            Id = Guid.NewGuid().ToString(),
            TeamName = "Test Team",
            TeamCreatorId = "admin-id",
            CreatedAt = DateTime.UtcNow
        };

        [TestInitialize]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockMapper = new Mock<IMapper>();

            handler = new GetAllTeamsQueryHandler(mockUnitOfWork.Object, mockMapper.Object);
        }

        [TestMethod]
        public async Task Handle_ReturnsMappedTeams_WhenTeamsExist()
        {
            // Arrange
            var teamsFromDb = new List<Gatam.Domain.ApplicationTeam>()
            {
                GLOBALTESTTEAM
            };

            mockUnitOfWork.Setup(u => u.TeamRepository.GetAllAsync(
               It.IsAny<Expression<Func<Gatam.Domain.ApplicationTeam, object>>>(),
               It.IsAny<Expression<Func<Gatam.Domain.ApplicationTeam, object>>>(),
               It.IsAny<Expression<Func<Gatam.Domain.ApplicationTeam, object>>>()))
               .ReturnsAsync(teamsFromDb);

            var mappedTeams = new List<Gatam.Domain.ApplicationTeam>()
            {
                GLOBALTESTTEAM
            };

            mockMapper.Setup(m => m.Map<IEnumerable<Gatam.Domain.ApplicationTeam>>(teamsFromDb))
                      .Returns(mappedTeams);

            var result = await handler.Handle(new GetAllTeamsQuery(), CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(GLOBALTESTTEAM.TeamName, result.First().TeamName);
        }

        [TestMethod]
        public async Task Handle_ReturnsEmptyList_WhenNoTeamsExist()
        {
            var emptyTeamsFromDb = new List<Gatam.Domain.ApplicationTeam>();

            mockUnitOfWork.Setup(u => u.TeamRepository.GetAllAsync(
                 It.IsAny<Expression<Func<Gatam.Domain.ApplicationTeam, object>>>(),
                 It.IsAny<Expression<Func<Gatam.Domain.ApplicationTeam, object>>>(),
                 It.IsAny<Expression<Func<Gatam.Domain.ApplicationTeam, object>>>()))
                 .ReturnsAsync(emptyTeamsFromDb);


            mockMapper.Setup(m => m.Map<IEnumerable<Gatam.Domain.ApplicationTeam>>(emptyTeamsFromDb))
                      .Returns(Enumerable.Empty<Gatam.Domain.ApplicationTeam>());

            var result = await handler.Handle(new GetAllTeamsQuery(), CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
