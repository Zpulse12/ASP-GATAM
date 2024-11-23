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
    public class QuestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        //[Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> GetAllQuestions()
        {
            IEnumerable<Question> questions = await _mediator.Send(new GetAllQuestionsQuery());
            return Ok(questions);
        }

        [HttpPost]
        //[Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question) 
        {
            Question createdQuestion = await _mediator.Send(new CreateQuestionCommand() { question = question});
            return Created("", createdQuestion);
        }

        [HttpGet("{questionId}")]
        //[Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> GetQuestionById(string questionId)
        {
            var questionById = await _mediator.Send(new GetQuestionByIdQuery { Id = questionId });
            if (questionById == null)
            {
                return NotFound("user niet gevonden");
            }

            return Ok(questionById);
        }

        [HttpPut("{questionId}")]
       //[Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> UpdateQuestion(string questionId, [FromBody] Question question)
        {
            var returnedQuestion = await _mediator.Send(new UpdateQuestionCommand() {  Question = question, Id = questionId, });
            return Ok(returnedQuestion);
        }

        [HttpGet("modules/{moduleId}/questions")]
        //[Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> GetQuestionByModuleId(string moduleId)
        {
            var questionByModuleId = await _mediator.Send(new GetAllQuestionByModuleIdQuery { ModuleId = moduleId });
            return Ok(questionByModuleId);
        }
    }
}
