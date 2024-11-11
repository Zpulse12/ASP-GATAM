using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting.CQRSTest.ApplicationUser
{
    [TestClass]
    public class AssignUserToBegeleiderCommandTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMapper> _mapperMock;
        private AssignUserToBegeleiderCommandHandler _handler;

        public AssignUserToBegeleiderCommandTest()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AssignUserToBegeleiderCommandHandler(_uowMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_VolgerId_Is_Empty()
        {
            var command = new AssignUserToBegeleiderCommand
            {
                VolgerId = string.Empty,
                BegeleiderId = "someBegeleiderId"
            };

            var validator = new AssignUserToBegeleiderCommandValidator();

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.PropertyName == "VolgerId"));
        }

        [TestMethod]
        public void Validator_Should_Fail_When_BegeleiderId_Is_Empty()
        {
            var command = new AssignUserToBegeleiderCommand
            {
                VolgerId = "someVolgerId",
                BegeleiderId = string.Empty
            };

            var validator = new AssignUserToBegeleiderCommandValidator();

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.PropertyName == "BegeleiderId"));
        }

        [TestMethod]
        public async Task Handler_Should_Assign_BegeleiderId_To_User()
        {
            var command = new AssignUserToBegeleiderCommand
            {
                VolgerId = "someVolgerId",
                BegeleiderId = "someBegeleiderId"
            };

            var user = new Gatam.Domain.ApplicationUser { Id = "someVolgerId" };

            _uowMock.Setup(u => u.UserRepository.FindById(command.VolgerId))
                    .ReturnsAsync(user);  

            _uowMock.Setup(u => u.UserRepository.Update(user))
                    .ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map<Gatam.Domain.ApplicationUser>(user))
                       .Returns(user); 

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(command.BegeleiderId, result.BegeleiderId);  
            _uowMock.Verify(u => u.UserRepository.Update(user), Times.Once); 
            _uowMock.Verify(u => u.commit(), Times.Once);  
        }

       
    }
}
