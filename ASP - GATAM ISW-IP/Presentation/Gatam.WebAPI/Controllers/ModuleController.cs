using Gatam.Application.CQRS.Module;
using Gatam.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.User;

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
        public async Task<IActionResult> CreateModule([FromBody] ModuleDTO module)
        {
            var result = await _mediator.Send(new CreateModuleCommand() { _module = module });
            return Created("", result);
        }

        [HttpDelete]
        [Route("delete/{moduleId}")]
        //[Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> DeleteModule(string moduleId)
        {
            var response = await _mediator.Send(new DeleteModuleCommand() { ModuleId = moduleId });
            if (response)
            {
                return Ok(response);
            }
            return NotFound("Module doesnt exists");
        }
    }
}
