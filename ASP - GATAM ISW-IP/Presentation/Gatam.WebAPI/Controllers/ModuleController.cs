using Gatam.Application.CQRS.Module;
using Gatam.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Gatam.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModuleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> GetAllModules()
        {
                var modules = await _mediator.Send(new GetAllModulesQuery());
                return Ok(modules);
        }

        [HttpPost]
        //[Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> CreateModule([FromBody] CreateModuleCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("", result);
        }
    }
}
