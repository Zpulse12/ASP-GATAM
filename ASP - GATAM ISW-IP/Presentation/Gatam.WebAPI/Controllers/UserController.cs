using Microsoft.AspNetCore.Mvc;
using Gatam.Application.CQRS;
using Gatam.Application.CQRS.Module;
using MediatR;
using Gatam.Domain;
using Gatam.Application.CQRS.User;
using Gatam.Application.CQRS.User.Roles;
using Microsoft.AspNetCore.Authorization;
using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.Application.CQRS.DTOS.RolesDTO;
using Gatam.Application.CQRS.DTOS.UsersDTO;
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
        [HttpPost("sync")]
        public async Task<IActionResult> SyncUsers()
        {
            var result = await _mediator.Send(new GetUsersWithSyncQuery());
            return Ok(result);
        }   

        [HttpGet]
        //[Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUsers()

        {
            var usersWithLocalStatus = await _mediator.Send(new GetAllUsersQuery());
            return Ok(usersWithLocalStatus);
        }

        [HttpGet("{userId}")]
       // [Authorize(Policy = "RequireManagementRole")]
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
        //[Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO user)
        {
            var result = await _mediator.Send(new CreateUserCommand() { _user = user });
            return Created("", result);
        }

        [HttpPatch]
        [Route("setactivestate")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> SetActiveState([FromBody] DeactivateUserCommand command)
        {
            return Ok(await _mediator.Send(new DeactivateUserCommand() { UserId = command.UserId, IsActive = command.IsActive }));
        }
        [HttpGet("{auth0UserId}/status")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetUserStatus(string userId)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(){UserId = userId});
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
        [Route("{id}/delete")]
        //[Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteUser(string id)
        {
           var response = await _mediator.Send(new DeleteUserCommand() { UserId = id });
           if (response)
           {
               return Ok(response);
           }
           return NotFound("User bestaat niet");
        }

        [HttpGet("{userId}/roles")]
        //[Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            List<string> roles = await _mediator.Send(new GetUserRolesQuery { UserId = userId });

            if (roles == null)
            {
                return NotFound("Geen rollen gevonden voor de opgegeven gebruiker.");
            }

            return Ok(roles);
        }
        
        [HttpPut("{userId}/AssignUserModule")]
        //[Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> AssignUserModule(string userId, [FromQuery] string moduleId)
        {
            var assignedUser = await _mediator.Send(new AssignModulesToUserCommand() {FollowerId = userId, ModuleId = moduleId });
            return Ok(assignedUser);
        }
        [HttpGet("{userId}/modules")]
       // [Authorize(Policy = "RequireVolgersRole")]
        public async Task<IActionResult> GetUserModules(string userId)
        {
            var query = new GetUserModulesQuery(userId);
            var modules = await _mediator.Send(query);
            if (modules == null || modules.Count == 0)
                return NotFound("No modules found for this user.");
            return Ok(modules);
        }

        [HttpGet("usersWithModules")]
        //[Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUsersWithModules()
        {
            var users = await _mediator.Send(new GetUsersWithModulesQuery());
            return Ok(users);
        }


        [HttpPost("{userId}/roles")]
        //[Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> AssignUserRole(string userId, [FromBody] RolesDTO roles)
        {
            var returnedUser = await _mediator.Send(new UpdateUserRoleCommand { Roles = roles, Id = userId});
            return Ok(returnedUser);
        }

        [HttpPut("{id}/users-assignment")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> AssignFollowerToMentor([FromBody] ApplicationUser user, string id)
        {

            var updateBegeleiderId = await _mediator.Send(new AssignUserToMentorCommand() { FollowerId = user.Id, MentorId = id });
            return Ok(updateBegeleiderId);

        }

        [HttpPut("users-assignment/removal")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> UnassignFollowerToMentor([FromBody] ApplicationUser user)
        {
            var updateBegeleiderId = await _mediator.Send(new UnassignUserCommand { FollowerId = user.Id });
            return Ok(updateBegeleiderId);
        }
        [HttpPatch("{id}/roles")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> RemoveUserRoles([FromBody] RolesDTO rolesDTO, string id)
        {
            var command = await _mediator.Send(new DeleteUserRolesCommand() { UserId = id, Roles = rolesDTO });
            return Ok(command);
        }
        [HttpGet("{mentorId}/mentor")]
        //[Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUsersForFollower([FromRoute] string mentorId)
        {
            var mentorDto = await _mediator.Send(new GetFollowersByMentorIdQuery() { MentorId = mentorId });
            return Ok(mentorDto);
        }
    }

}
