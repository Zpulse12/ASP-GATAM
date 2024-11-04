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
            try
            {
                var auth0Users = await _mediator.Send(new GetAllUsersQuery());

                var usersWithLocalStatus = new List<UserDTO>();
        
                foreach (var auth0User in auth0Users)
                {
                    var localUser = await _mediator.Send(new FindUserByIdQuery(auth0User.Id));

                    if (localUser == null)
                    {
                        var newUser = new ApplicationUser
                        {
                            Id = auth0User.Id,
                            IsActive = true
                        };
                        await _mediator.Send(new CreateUserCommand { _user = newUser });
                        auth0User.IsActive = true;
                    }
                    else
                    {
                        auth0User.IsActive = localUser.IsActive;
                    }
            
                    usersWithLocalStatus.Add(auth0User);
                }

                return Ok(usersWithLocalStatus);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error synchronizing users: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }





        [HttpGet("findbyid/{auth0UserId}")]
        public async Task<IActionResult> FindById(string auth0UserId)
        {
            try
            {
                var user = await _mediator.Send(new FindUserByIdQuery(auth0UserId));

                if (user == null) 
                {
                    return NotFound($"User with Auth0 ID '{auth0UserId}' not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching user with Auth0 ID '{auth0UserId}': {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
        [Authorize(Roles = RoleMapper.Admin)]
        public async Task<IActionResult> SetActiveState(string id, [FromBody] DeactivateUserCommand command)
        {
            command.UserId = id;

            var user = await _mediator.Send(new DeactivateUserCommand() { UserId = id, IsActive = command.IsActive });

            if (user == null)
            {
                return NotFound("User bestaat niet");
            }

            return Ok(user);
        }
        [HttpGet("status/{auth0UserId}")]
        public async Task<IActionResult> GetUserStatus(string auth0UserId)
        {
            try
            {
                var user = await _mediator.Send(new FindUserByIdQuery(auth0UserId));

                if (user == null)
                {
                    return NotFound("User does not exist");
                }

                return Ok(new { IsActive = user.IsActive });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching user with Auth0 ID '{auth0UserId}': {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{userId}")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDTO user)
        {
            if (userId != user.Id)
            {
                return BadRequest("The user ID in the URL does not match the user ID in the body.");
            }
            var returnedUser = await _mediator.Send(new UpdateUserCommand() { Id = userId, User = user });
            return Ok(returnedUser);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = RoleMapper.Admin)]
        public async Task<IActionResult> DeleteUser(string id)
        {
           var response = await _mediator.Send(new DeleteUserCommand() { UserId = id });
           if (response)
           {
               return Ok(response);
           }
           return NotFound("User doesnt exists");
        }
        [HttpGet("private-scoped")]
        [Authorize("read:admin")]
        public IActionResult Scoped()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated and have a scope of read:messages to see this."
            });
        }
    }

}
