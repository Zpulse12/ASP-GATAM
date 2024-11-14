using Gatam.Application.CQRS;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.WebAPI.Controllers;
using MediatR;

namespace UnitTesting.CQRSTest.ApplicationUser
{
    [TestClass]
    public class GetAllUsersWithBegeleiderIdControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private UserController _controller; 

        [TestInitialize]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task GetAllUsersWithBegeleiderId_Should_Return_Ok_With_Data()
        {
            var mockUsers = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = "1",
                    Nickname = "user1",
                    BegeleiderId = "begeleider1",
                    Email = null,
                    RolesIds = null
                },
                new UserDTO
                {
                    Id = "2",
                    Nickname = "user2",
                    BegeleiderId = "begeleider2",
                    Email = null,
                    RolesIds = null
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllUsersWithBegeleiderIdQuery>(), default))
                         .ReturnsAsync(mockUsers);

            var result = await _controller.GetAllUsersWithBegeleiderId();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); 
            var users = okResult.Value as List<UserDTO>;
            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual("user1", users[0].Nickname); 
        }

        [TestMethod]
        public async Task GetAllUsersWithBegeleiderId_Should_Return_Empty_List_When_No_Users_Found()
        {
          
            var mockUsers = new List<UserDTO>(); 

           
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllUsersWithBegeleiderIdQuery>(), default))
                         .ReturnsAsync(mockUsers);

           
            var result = await _controller.GetAllUsersWithBegeleiderId();

           
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); 
            var users = okResult.Value as List<UserDTO>;
            Assert.IsNotNull(users);
            Assert.AreEqual(0, users.Count); 
        }
    }
}
