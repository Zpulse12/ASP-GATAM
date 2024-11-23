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
       
        [HttpGet("user/{userId}/modules")]
        public async Task<IActionResult> GetUserModules(string userId)
        {
            var query = new GetUserModuleByUserQuery(userId);
            var modules = await _mediator.Send(query);
            return Ok(modules);
        }

        
        [HttpGet("{usermoduleId}")]
        public async Task<IActionResult> GetUserModuleById(string usermoduleId)
        {
            var query = new FindUserModuleIdQuery() { UserModuleId = usermoduleId };
            var usermodule = await _mediator.Send(query);
            return Ok(usermodule);
        }

        [HttpPut("{usermoduleId}/answers")]
        public async Task<IActionResult> SubmitAnswers(string userModuleId, [FromBody] List<UserAnswer> userAnswers)
        {
            var updateCommand = new UpdateUserModuleStatus() {UserModuleId=userModuleId, State = UserModuleState.InProgress };
            await _mediator.Send(updateCommand);

            var command = new SubmitUserAnswersCommand() { UserAnwsers = userAnswers, UserModuleId = userModuleId };
            var userModule = await _mediator.Send(command);

            return Ok(userModule);
        }
    }
}
