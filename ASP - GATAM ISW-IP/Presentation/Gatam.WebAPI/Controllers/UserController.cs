using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gatam.Application.CQRS;
using MediatR;
using Gatam.Domain;
using Gatam.WebAPI.Extensions;
using System.Diagnostics;
using Gatam.Application.CQRS.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Gatam.Application.Extensions;
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
            var usersWithLocalStatus = await _mediator.Send(new GetUsersWithSyncQuery());
            return Ok(usersWithLocalStatus);
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
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
        {
            var result = await _mediator.Send(new CreateUserCommand() { _user = user });
            return result == null ? BadRequest(result) : Created("", result);
        }

        [HttpPatch]
        [Route("setactivestate/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SetActiveState(string id, [FromBody] DeactivateUserCommand command)
        {
            command.UserId = id;
            return Ok(await _mediator.Send(new DeactivateUserCommand() { UserId = id, IsActive = command.IsActive }));
        }
        [HttpGet("status/{auth0UserId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetUserStatus(string auth0UserId)
        {
            var user = await _mediator.Send(new FindUserByIdQuery(auth0UserId));
            return Ok(new { IsActive = user.IsActive });
        }

        [HttpPut("{userId}")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDTO user)
        {
            
            var returnedUser = await _mediator.Send(new UpdateUserCommand() {User = user, Id = userId });
            return Ok(returnedUser);
        }

        

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
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

        public async Task<IActionResult> AssignUserRole(string userId, [FromBody] UserDTO user)
        {
            
            var returnedUser = await _mediator.Send(new AssignUserRoleCommand { User = user, Id = userId});
            return Ok(returnedUser);
        }
    }

}
