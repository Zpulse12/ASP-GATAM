using Gatam.Application.CQRS;
using Gatam.Application.CQRS.Module;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllModules()
        {
            var modules = await _mediator.Send(new GetAllModulesQuery());

            return Ok(modules);
        }
    }
}
