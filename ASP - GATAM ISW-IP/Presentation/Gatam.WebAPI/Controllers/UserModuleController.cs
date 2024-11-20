using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Application.CQRS.Questions;
using Gatam.Application.CQRS.User;
using Gatam.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gatam.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserModuleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserModuleController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UserModule> usermodules = await _mediator.Send(new GetAllUserModulesQuery());
            return Ok(usermodules);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserModules(string userId)
        {
            var query = new GetUserModuleByUserQuery(userId);
            var modules = await _mediator.Send(query);
            if (modules == null || modules.Count == 0)
                return NotFound("No modules found for this user.");
            return Ok(modules);
        }

        [HttpGet("usermodule/{usermoduleId}")]
        public async Task<IActionResult> GetUserModuleById(string usermoduleId)
        {
            var query = new FindUserModuleIdQuery() { UserModuleId = usermoduleId };
            var usermodule = await _mediator.Send(query);
            return Ok(usermodule);
        }
    }
}
