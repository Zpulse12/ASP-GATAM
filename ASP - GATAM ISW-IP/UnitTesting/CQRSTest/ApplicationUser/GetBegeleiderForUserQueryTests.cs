using AutoMapper;
using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser;
[TestClass]
public class GetBegeleiderForUserQueryTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IMapper> _mockMapper;

    [TestInitialize]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
    }

    [TestMethod]
    public async Task Handle_ShouldReturnUserDTO_WhenBegeleiderExists()
    {
        var begeleider = new Gatam.Domain.ApplicationUser { Id = "begeleider123", UserName = "BegeleiderNaam" };
        var user = new Gatam.Domain.ApplicationUser { Id = "user123", BegeleiderId = "begeleider123" };
        var userDto = new UserDTO
        {
            Id = "begeleider123",
            Nickname = "BegeleiderNaam",
            Email = null,
            RolesIds = null
        };

        _mockUnitOfWork.Setup(u => u.UserRepository.FindById("user123"))
            .ReturnsAsync(user);
        _mockUnitOfWork.Setup(u => u.UserRepository.FindById("begeleider123"))
            .ReturnsAsync(begeleider);
        _mockMapper.Setup(m => m.Map<UserDTO>(begeleider)).Returns(userDto);

        var handler = new GetBegeleiderForUserQuery.GetBegeleiderForUserQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetBegeleiderForUserQuery("user123"), CancellationToken.None);

        Assert.IsNotNull(result);
        Assert.AreEqual("BegeleiderNaam", result.Nickname);
    }
}
