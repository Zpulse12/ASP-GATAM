using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gatam.Application.CQRS;
using MediatR;
using Gatam.Domain;
using Gatam.WebAPI.Extensions;
using System.Diagnostics;
using Gatam.Application.CQRS.User;
using Microsoft.AspNetCore.Authorization;
namespace Gatam.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUsersById(string userId)
        {
            var userById = await _mediator.Send(new GetUserByIdQuery { UserId = userId });
            if(userById == null)
            {
                return NotFound("user niet gevonden");
            }

            return Ok(userById);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
        {
            var result = await _mediator.Send(new CreateUserCommand() { _user = user });
            Debug.WriteLine(result);
            return result == null ? BadRequest(result) : Created("", result);
        }

        [HttpPut]
        [Route("setactivestate/{id}")]
        public async Task<IActionResult> SetActiveState(string id, [FromBody] DeactivateUserCommand command)
        {
            command._userId = id;

            var user = await _mediator.Send(new DeactivateUserCommand() { _userId = id, IsActive = command.IsActive });

            if (user == null)
            {
                return NotFound("User bestaat niet");
            }

            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDTO user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId != user.Id)
            {
                return BadRequest("The user ID in the URL does not match the user ID in the body.");
            }
            var returnedUser = await _mediator.Send(new UpdateUserCommand() { Id = userId, User = user });
            return Ok(returnedUser);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
           var response = await _mediator.Send(new DeleteUserCommand() { UserId = id });
           if (response)
           {
               return Ok(response);
           }
           return NotFound("User doesnt exists");
        }

        [HttpGet("{userId}/roles")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _mediator.Send(new GetUserByIdQuery { UserId = userId });

            if (roles == null)
            {
                return NotFound("Geen rollen gevonden voor de opgegeven gebruiker.");
            }

            return Ok(roles);
        }


        [HttpPut("{userId}/roles")]
        [Authorize(Policy = "RequireManagementRole")]

        public async Task<IActionResult> UpdateUserRole(string userId, [FromBody] UserDTO user)
        {
            
            var returnedUser = await _mediator.Send(new UpdateUserCommand { User = user, Id = userId});
            return Ok(returnedUser);
        }
    }

}
