using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gatam.Application.CQRS.User;

namespace UnitTesting.CQRSTest.TeamInvitation
{
    using AutoMapper;
    using Gatam.Application.CQRS;
    using Gatam.Application.Interfaces;
    using Gatam.Domain;
    using MediatR;
    using Moq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http.HttpResults;
    using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

    namespace UnitTesting.CQRSTest.TeamInvitation
    {
        [TestClass]
        public class GetAllInvitationsQueryTest
        {
            private Mock<IUnitOfWork> mockUnitOfWork;
            private Mock<IMapper> mockMapper;
            private IRequestHandler<GetAllInvitationsQuery, IEnumerable<Gatam.Domain.TeamInvitation>> handler;

            private ApplicationUser GLOBALTESTUSER;  // Verplaats de declaratie

            private List<Gatam.Domain.TeamInvitation> GLOBALTESTINVITATIONS;
            [TestInitialize]
            public void Setup()
            {
                // Initialiseer de GLOBALTESTUSER hier
                GLOBALTESTUSER = new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Email = "admin@app.com"
                };

                // Initialiseer de GLOBALTESTINVITATIONS hier
                GLOBALTESTINVITATIONS = new List<Gatam.Domain.TeamInvitation>()
            {
                new Gatam.Domain.TeamInvitation()
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationTeamId = Guid.NewGuid().ToString(),
                    UserId = GLOBALTESTUSER.Id,
                    isAccepted = true,
                    CreatedAt = DateTime.UtcNow,
                    ResponseDateTime = DateTime.UtcNow
                },
                new Gatam.Domain.TeamInvitation()
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationTeamId = Guid.NewGuid().ToString(),
                    UserId = GLOBALTESTUSER.Id,
                    isAccepted = false,
                    CreatedAt = DateTime.UtcNow,
                    ResponseDateTime = DateTime.UtcNow
                }
            };

                mockUnitOfWork = new Mock<IUnitOfWork>();
                mockMapper = new Mock<IMapper>();

                handler = new GetAllInvitationsQueryHandler(mockUnitOfWork.Object, mockMapper.Object);
            }
            [TestMethod]
            public async Task Handle_ReturnsMappedInvitations_WhenInvitationsExist()
            {
                mockUnitOfWork.Setup(u => u.TeamInvitationRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Gatam.Domain.TeamInvitation, object>>>()))
                    .ReturnsAsync(GLOBALTESTINVITATIONS);

                mockMapper.Setup(m => m.Map<IEnumerable<Gatam.Domain.TeamInvitation>>(GLOBALTESTINVITATIONS))
                          .Returns(GLOBALTESTINVITATIONS);
                var result = await handler.Handle(new GetAllInvitationsQuery(), CancellationToken.None);
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(GLOBALTESTINVITATIONS[0].Id, result.First().Id);
            }

            [TestMethod]
            public async Task Handle_ReturnsEmptyList_WhenNoInvitationsExist()
            {
                var emptyInvitationsFromDb = new List<Gatam.Domain.TeamInvitation>();
                mockUnitOfWork.Setup(u => u.TeamInvitationRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Gatam.Domain.TeamInvitation, object>>>()))
                    .ReturnsAsync(emptyInvitationsFromDb);

                mockMapper.Setup(m => m.Map<IEnumerable<Gatam.Domain.TeamInvitation>>(emptyInvitationsFromDb))
                          .Returns(Enumerable.Empty<Gatam.Domain.TeamInvitation>());
                var result = await handler.Handle(new GetAllInvitationsQuery(), CancellationToken.None);
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count());
            }
        }
    }

}
