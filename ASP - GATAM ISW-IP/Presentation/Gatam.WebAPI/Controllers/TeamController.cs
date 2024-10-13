using Gatam.Application.CQRS;
using Gatam.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gatam.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
                var teams = await _mediator.Send(new GetAllTeamsQuery());
                return Ok(teams);
        }




    }
}
