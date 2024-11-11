using Microsoft.AspNetCore.Mvc;
using Gatam.Application.CQRS;
using Gatam.Application.CQRS.Module;
using MediatR;
using Gatam.Domain;
using Gatam.Application.CQRS.User;
using Gatam.Application.CQRS.User.Roles;
using Microsoft.AspNetCore.Authorization;
using Gatam.Application.CQRS.User.BegeleiderAssignment;
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
            var result = await _mediator.Send(new CreateUserCommand() { User = user });
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
            List<string> roles = await _mediator.Send(new GetUserRolesQuery { UserId = userId });

            if (roles == null)
            {
                return NotFound("Geen rollen gevonden voor de opgegeven gebruiker.");
            }

            return Ok(roles);
        }
        [HttpPut("AssignUserModule/{userId}")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> AssignUserModule(string userId, [FromQuery] string moduleId)
        {
            var assignedUser = await _mediator.Send(new AssignModulesToUserCommand() {VolgerId = userId, ModuleId = moduleId });
            return Ok(assignedUser);
        }
        [HttpGet("modules/{userId}")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUserModules(string userId)
        {
            var query = new GetUserModulesQuery(userId);
            var modules = await _mediator.Send(query);
            if (modules == null || modules.Count == 0)
                return NotFound("No modules found for this user.");
            return Ok(modules);
        }

        [HttpGet("usersWithModules")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetUsersWithModules()
        {
            var users = await _mediator.Send(new GetUsersWithModulesQuery());
            return Ok(users);
        }


        [HttpPut("{userId}/roles")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> AssignUserRole(string userId, [FromBody] UserDTO user)
        {
            
            var returnedUser = await _mediator.Send(new AssignUserRoleCommand { User = user, Id = userId});
            return Ok(returnedUser);
        }


        [HttpGet("AssignUsersToBegeleider")]
        [Authorize(Policy = "RequireManagementRole")]

        public async Task<IActionResult> GetAllUsersWithBegeleiderId()
        {
            var assignUsersToBegeleider = await _mediator.Send(new GetAllUsersWithBegeleiderIdQuery());
            return Ok(assignUsersToBegeleider);
        }

        [HttpPut("AssignUsersToBegeleider/{id}")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> AssignUsersToBegeleider([FromBody] ApplicationUser user, string id)
        {
            var volger = await _mediator.Send(new FindUserByIdQuery(id));
            if(volger != null)
            {
                var updateBegeleiderId = await _mediator.Send(new AssignUserToBegeleiderCommand() { VolgerId = user.Id, BegeleiderId = id});
                return Ok(updateBegeleiderId);

            }
            return NotFound();
        }

        [HttpPut("UnassignUsersToBegeleider/{id}")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> UnassignUsersToBegeleider(string id)
        {
            var volger = await _mediator.Send(new FindUserByIdQuery(id));
            if (volger != null)
            {
                var updateBegeleiderId = await _mediator.Send(new UnassignUserCommand() { VolgerId = id });
                return Ok(updateBegeleiderId);

            }
            return NotFound();
        }




    }

}
