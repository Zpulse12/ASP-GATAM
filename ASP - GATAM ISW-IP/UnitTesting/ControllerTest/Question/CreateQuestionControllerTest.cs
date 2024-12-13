//using Moq;
//using Microsoft.AspNetCore.Mvc;
//using Gatam.Application.CQRS.Questions;
//using Gatam.Domain;
//using Gatam.WebAPI.Controllers;
//using MediatR;
//using FluentValidation;
//using Gatam.Application.Behaviours;

//namespace UnitTesting.ControllerTest.Question
//{
//    [TestClass]
//    public class CreateQuestionControllerTest
//    {
//        private Mock<IMediator> _mediatorMock;
//        private QuestionController _questionController;
//        private IValidator<CreateQuestionCommand> _validator;
//        private ValidationBehaviour<CreateQuestionCommand, Gatam.Application.CQRS.DTOS.QuestionsDTO.QuestionDTO> _validationBehavior;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mediatorMock = new Mock<IMediator>();
//            _questionController = new QuestionController(_mediatorMock.Object);
//        }

//        [TestMethod]
//        public async Task CreateQuestionReturns_Question()
//        {

//            List<QuestionAnswer> answerList = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "OPEN", AnswerValue = "OPEN" } }; 
//            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Veryyy loong title?", QuestionType = (short)QuestionType.OPEN, Answers = answerList  };
//            //_mediatorMock.Setup(setup => setup.Send(It.IsAny<CreateQuestionCommand>(), default)).ReturnsAsync(question);
//            IActionResult result = await _questionController.CreateQuestion(question);
//            CreatedResult? createdResult = result as CreatedResult;
//            Assert.IsNotNull(createdResult);
//            Assert.AreEqual(201, createdResult.StatusCode);
//            Assert.AreEqual(question, createdResult.Value);
//        }
//        [TestMethod]
//        public async Task CreateQuestion_failsOn_emptyTitle()
//        {
//            List<QuestionAnswer> answerList = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "OPEN", AnswerValue = "OPEN" } };
//            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "", QuestionType = (short)QuestionType.OPEN, Answers = answerList };
//            CreateQuestionCommand invalidCommand = new CreateQuestionCommand() { question = question };
//            //await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
//           // await _validationBehavior.Handle(invalidCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None));
//        }
//        [TestMethod]
//        public async Task CreateQuestion_failsOn_shortTitle()
//        {
//            List<QuestionAnswer> answerList = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "OPEN", AnswerValue = "OPEN" } };
//            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Veryyy?", QuestionType = (short)QuestionType.OPEN, Answers = answerList };
//            CreateQuestionCommand invalidCommand = new CreateQuestionCommand() { question = question };
//            //await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
//            //await _validationBehavior.Handle(invalidCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None));
//        }
//        [TestMethod]
//        public async Task CreateQuestion_failsOn_missingQuestionType()
//        {
//            List<QuestionAnswer> answerList = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "OPEN", AnswerValue = "OPEN" } };
//            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Veryyy loong title?", Answers = answerList };
//            CreateQuestionCommand invalidCommand = new CreateQuestionCommand() { question = question };
//            //await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
//            //await _validationBehavior.Handle(invalidCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None));
//        }
//        [TestMethod]
//        public async Task CreateQuestion_failsOn_emptyAnswerList()
//        {
//            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Veryyy loong title?", QuestionType = (short)QuestionType.OPEN };
//            CreateQuestionCommand invalidCommand = new CreateQuestionCommand() { question = question };
//            //await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
//            //await _validationBehavior.Handle(invalidCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None));
//        }
//    }
//}
