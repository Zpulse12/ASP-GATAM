using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.Domain;
using Gatam.Application.Interfaces;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;

namespace UnitTesting.CQRSTest.ApplicationUser
{
   

    [TestClass]
    public class UnassignUserCommandTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMapper> _mapperMock;
        private UnassignUserCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            // Maak mocks voor de afhankelijkheden
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            // Maak de handler aan met de gemockte afhankelijkheden
            _handler = new UnassignUserCommandHandler(_uowMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task Handle_UserExists_UnassignsUserSuccessfully()
        {
            // Arrange
            var volgerId = "volgerId";
            var user = new Gatam.Domain.ApplicationUser
            {
                Id = volgerId,
                BegeleiderId = "begeleiderId"  // Gebruiker heeft een begeleider
            };

            // Mock het gedrag van de UserRepository
            _uowMock.Setup(u => u.UserRepository.FindById(volgerId)).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(new UnassignUserCommand { VolgerId = volgerId }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(volgerId, result.Id);
            Assert.IsNull(result.BegeleiderId); // BegeleiderId moet nu null zijn

            // Verifieer of de Update methode werd aangeroepen
            _uowMock.Verify(u => u.UserRepository.Update(It.Is<Gatam.Domain.ApplicationUser>(x => x.BegeleiderId == null)), Times.Once);

            // Verifieer of commit werd aangeroepen
            _uowMock.Verify(u => u.commit(), Times.Once);
        }


    }

}
