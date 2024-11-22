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
        [Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> GetAllQuestions()
        {
            IEnumerable<Question> questions = await _mediator.Send(new GetAllQuestionsQuery());
            return Ok(questions);
        }

        [HttpPost("settings")]
        [Authorize(Policy = "RequireManagementRole")]

        public async Task<IActionResult> CreateSetting([FromBody] CreateSettingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question) 
        {
            Question createdQuestion = await _mediator.Send(new CreateQuestionCommand() { question = question});
            return Created("", createdQuestion);
        }
        [HttpGet("visible/{followerId}")]
        [Authorize(Policy = "RequireManagementRole")]
        public async Task<IActionResult> GetVisibleQuestionsForFollower(string followerId)
        {
            var questions = await _mediator.Send(new GetVisibleQuestionsForFollowerQuery { FollowerId = followerId });
            return Ok(questions);
        }

        [HttpGet("{questionId}")]
        [Authorize(Policy = "RequireMakerRole")]
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
        [Authorize(Policy = "RequireMakerRole")]
        public async Task<IActionResult> UpdateQuestion(string questionId, [FromBody] Question question)
        {
            var returnedQuestion = await _mediator.Send(new UpdateQuestionCommand() {  Question = question, Id = questionId, });
            return Ok(returnedQuestion);
        }

        // [HttpGet("settings/usermodule/{userModuleId}")]
        // public async Task<IActionResult> GetQuestionSettingsForUserModule(string userModuleId)
        // {
        //     var settings = await _mediator.Send(new GetQuestionSettingsForUserModuleQuery { UserModuleId = userModuleId });
        //     return Ok(settings);
        // }

        [HttpPut("visibility")]
        // [Authorize(Policy = "RequireManagementRole")]

        public async Task<IActionResult> UpdateVisibility([FromBody] UpdateQuestionVisibilityCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
