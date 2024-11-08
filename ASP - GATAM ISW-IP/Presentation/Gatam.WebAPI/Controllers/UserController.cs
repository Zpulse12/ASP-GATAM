using Microsoft.AspNetCore.Mvc;
using Gatam.Application.CQRS;
using MediatR;
using Gatam.Domain;
using Gatam.Application.CQRS.User;
using Gatam.Application.CQRS.User.Roles;
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
        [Route("setactivestate")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SetActiveState([FromBody] DeactivateUserCommand command)
        {
            return Ok(await _mediator.Send(new DeactivateUserCommand() { UserId = command.UserId, IsActive = command.IsActive }));
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
            List<string> roles = await _mediator.Send(new GetUserRolesQuery { UserId = userId });

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
