using AutoMapper;
using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser
{
    [TestClass]
    public class AssignUserToBegeleiderCommandTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMapper> _mapperMock;
        private AssignUserToMentorCommandHandler _handler;

        public AssignUserToBegeleiderCommandTest()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AssignUserToMentorCommandHandler(_uowMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_VolgerId_Is_Empty()
        {
            var command = new AssignUserToMentorCommand
            {
                FollowerId = string.Empty,
                MentorId = "someBegeleiderId"
            };

            var validator = new AssignUserToMentorCommandValidator();

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.PropertyName == "VolgerId"));
        }

       

        [TestMethod]
        public async Task Handler_Should_Assign_BegeleiderId_To_User()
        {
            var command = new AssignUserToMentorCommand
            {
                FollowerId = "someVolgerId",
                MentorId = "someBegeleiderId"
            };

            var user = new Gatam.Domain.ApplicationUser { Id = "someVolgerId" };

            _uowMock.Setup(u => u.UserRepository.FindById(command.FollowerId))
                    .ReturnsAsync(user);  

            _uowMock.Setup(u => u.UserRepository.Update(user))
                    .ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map<Gatam.Domain.ApplicationUser>(user))
                       .Returns(user); 

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(command.MentorId, result.MentorId);  
            _uowMock.Verify(u => u.UserRepository.Update(user), Times.Once); 
            _uowMock.Verify(u => u.Commit(), Times.Once);  
        }

       
    }
}
