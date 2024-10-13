using Gatam.Application.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatam.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationTeamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvitationTeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvitationsTeams()
        {
            var invitationsteams = await _mediator.Send(new GetAllInvitationsQuery());
            return Ok(invitationsteams);
        }
    }
}
