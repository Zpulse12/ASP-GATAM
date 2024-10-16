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


        [HttpPut]
        [Route("acceptInvitation/{id}")]
        public async Task<IActionResult> AcceptInvitation(string id, [FromBody] AcceptTeamInvitationCommand command)
        {
            command._teaminvitationId = id;

            var invitation = await _mediator.Send(new AcceptTeamInvitationCommand() { _teaminvitationId = id, IsAccepted = command.IsAccepted });

            if (invitation == null)
            {
                return NotFound("invitation bestaat niet");
            }

            return Ok(invitation);
        }
    }


}
