using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gatam.Application.CQRS;
using MediatR;
using Gatam.Domain;
using Gatam.WebAPI.Extensions;
using System.Diagnostics;
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
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
        {
            var result = await _mediator.Send(new CreateUserCommand() { _user = user });
            Debug.WriteLine(result);
            return result == null ? BadRequest(result) : Created("", result);
        }

        [HttpPut]
        [Route("deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(string id, [FromBody] DeactivateUserCommand command)
        {
            command._userId = id;

            var user = await _mediator.Send(command);

            if(user == null) 
            {
                return NotFound("User betsaat niet");
            }

            return Ok(user);
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
    }

}
