﻿using Gatam.Application.CQRS;
using Gatam.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [HttpPost]
        public async Task<IActionResult> AddUserToTeam([FromBody] TeamInvitationDTO invitation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new AddToTeamCommand() { _teamInvitation = invitation });
            Debug.WriteLine(result);
            Debug.WriteLine("invitation: " + invitation);
            return result == null ? BadRequest(result) : Created("", result);
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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTeamInvitation(string id)
        {
            var response = await _mediator.Send(new DeleteTeamInvitationCommand() { TeamInvitationId = id });
            if (response)
            {
                return Ok(response);
            }
            return NotFound("invitation doesnt exists");

        }

    }


}



