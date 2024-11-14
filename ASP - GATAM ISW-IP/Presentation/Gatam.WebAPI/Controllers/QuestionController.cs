using Gatam.Application.CQRS.Questions;
using Gatam.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gatam.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> GetAllQuestions()
        {
            IEnumerable<Question> questions = await _mediator.Send(new GetAllQuestionsQuery());
            return Ok(questions);
        }

        [HttpPost]
        [Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question) 
        {
            Question createdQuestion = await _mediator.Send(new CreateQuestionCommand() { question = question});
            return Created("", createdQuestion);
        }
    }
}
