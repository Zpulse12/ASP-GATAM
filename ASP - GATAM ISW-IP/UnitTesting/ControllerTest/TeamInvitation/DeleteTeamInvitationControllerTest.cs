using Gatam.Application.CQRS; // Zorg ervoor dat deze namespace correct is
using Gatam.WebAPI.Controllers; // Zorg ervoor dat deze namespace correct is
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;

namespace UnitTesting.ControllerTest.TeamInvitation
{
    [TestClass]
    public class DeleteTeamInvitationControllerTest
    {
        private Mock<IMediator>? mediator;
        private InvitationTeamController? controller;

        [TestInitialize]
        public void Setup()
        {
            mediator = new Mock<IMediator>();
            controller = new InvitationTeamController(mediator.Object);
        }

        [TestMethod]
        public async Task DeleteTeamInvitation_Test_Success()
        {
            // Arrange
            string invitationId = Guid.NewGuid().ToString();
            mediator.Setup(m => m.Send(It.IsAny<DeleteTeamInvitationCommand>(), default))
                .ReturnsAsync(true); // Simuleer een succesvolle delete operatie

            // Act
            var result = await controller.DeleteTeamInvitation(invitationId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); // Gebruik Assert.AreEqual in plaats van Assert.Equals
            Assert.AreEqual(true, okResult.Value); // Gebruik Assert.AreEqual in plaats van Assert.Equals
        }

        [TestMethod]
        public async Task DeleteTeamInvitation_Test_NotFound()
        {
            // Arrange
            string invitationId = Guid.NewGuid().ToString();
            mediator.Setup(m => m.Send(It.IsAny<DeleteTeamInvitationCommand>(), default))
                .ReturnsAsync(false); // Simuleer dat de uitnodiging niet gevonden is

            // Act
            var result = await controller.DeleteTeamInvitation(invitationId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode); // Gebruik Assert.AreEqual in plaats van Assert.Equals
            Assert.AreEqual("invitation doesnt exists", notFoundResult.Value); // Gebruik Assert.AreEqual in plaats van Assert.Equals
        }
    }
}