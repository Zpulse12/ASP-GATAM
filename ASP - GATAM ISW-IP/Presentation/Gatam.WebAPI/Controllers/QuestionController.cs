using Gatam.Application.CQRS.Questions;
using Gatam.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatam.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            return Ok("YES");
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question) 
        {
            Question createdQuestion = await _mediator.Send(new CreateQuestionCommand() { question = question});
            return Ok(createdQuestion);
        }
    }
}
