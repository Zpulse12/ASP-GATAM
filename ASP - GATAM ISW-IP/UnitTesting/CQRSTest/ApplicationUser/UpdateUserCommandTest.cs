using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Gatam.Application.CQRS;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser;
[TestClass]
public class UpdateUserCommandTest
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private UpdateUserCommandHandler _handler;



        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateUserCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldUpdateUser_WhenUserExists()
        {
            var userId = "12345";
            var user = new Gatam.Domain.ApplicationUser
            {
                Id = userId,
                UserName = "OriginalUser",
                Email = "original@example.com",
                Role = ApplicationUserRoles.STUDENT,
                IsActive = true
            };

            var updatedUserDto = new UserDTO
            {
                Id = userId,
                Username = "UpdatedUser",
                Email = "updated@example.com",
                _role = ApplicationUserRoles.ADMIN,
                IsActive = false
            };

            _mockUnitOfWork.Setup(uow => uow.UserRepository.FindById(userId))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(uow => uow.UserRepository.Update(It.IsAny<Gatam.Domain.ApplicationUser>()))
                .ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map(updatedUserDto, user))
                .Callback<UserDTO, Gatam.Domain.ApplicationUser>((src, dest) =>
                {
                    dest.UserName = src.Username;
                    dest.Email = src.Email;
                    dest.Role = src._role;
                    dest.IsActive = src.IsActive;
                });

            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<Gatam.Domain.ApplicationUser>()))
                .Returns(updatedUserDto);

            var command = new UpdateUserCommand()
            {
                 Id= userId,
                User = updatedUserDto
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUserDto.Username, result.Username);
            Assert.AreEqual(updatedUserDto.Email, result.Email);
            Assert.AreEqual(updatedUserDto._role, result._role);
            Assert.AreEqual(updatedUserDto.IsActive, result.IsActive);

            _mockUnitOfWork.Verify(uow => uow.commit(), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.UserRepository.Update(It.IsAny<Gatam.Domain.ApplicationUser>()), Times.Once);
        }
}