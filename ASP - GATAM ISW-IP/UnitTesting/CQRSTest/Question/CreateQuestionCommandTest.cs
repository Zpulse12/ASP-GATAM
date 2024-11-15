using FluentValidation;
using Gatam.Application.Behaviours;
using Gatam.Application.CQRS.Questions;
using Gatam.Application.Exceptions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;


namespace UnitTesting.CQRSTest.Question
{
    [TestClass]
    public class CreateQuestionCommandTest
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private CreateQuestionCommandHandler _handler;
        private ValidationBehaviour<CreateQuestionCommand, Gatam.Domain.Question> _commandBehaviour;
        [TestInitialize]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateQuestionCommandHandler(_unitOfWork.Object);
            CreateQuestionCommandValidator validator = new CreateQuestionCommandValidator();
            IEnumerable<IValidator<CreateQuestionCommand>> validators = new List<IValidator<CreateQuestionCommand>>() { validator };
            _commandBehaviour = new ValidationBehaviour<CreateQuestionCommand, Gatam.Domain.Question>(validators);
        }
        [TestMethod]
        public async Task Handler_ShouldReturnValidQuestion_WhenCreated()
        {
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Wat heb je gegeten vandaag?", Answers = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "Pizza", AnswerValue = "Pizza" } } };
            CreateQuestionCommand questionCommand = new CreateQuestionCommand() { question = question };
            _unitOfWork.Setup(uow => uow.QuestionRepository.Create(It.IsAny<Gatam.Domain.Question>())).Returns(Task.FromResult(question));
            Gatam.Domain.Question result = await _handler.Handle(questionCommand, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual("Wat heb je gegeten vandaag?", result.QuestionTitle);
            Assert.AreEqual("Pizza", result.Answers.First().AnswerValue);
        }
        [TestMethod]
        public async Task QuestionTitle_shouldBe_Uniform()
        {
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "hoeveel eitjes wil je bij je spek", Answers = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "2", AnswerValue = "Pizza" } } };
            CreateQuestionCommand questionCommand = new CreateQuestionCommand() { question = question };
            _unitOfWork.Setup(uow => uow.QuestionRepository.Create(It.IsAny<Gatam.Domain.Question>())).Returns(Task.FromResult(question));
            Gatam.Domain.Question result = await _handler.Handle(questionCommand, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual("Hoeveel eitjes wil je bij je spek?", result.QuestionTitle);
            Assert.AreEqual("2", result.Answers.First().Answer);
        }

        [TestMethod]
        public async Task Handler_shouldFail_onEmptyTitle()
        {
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "", Answers = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "2", AnswerValue = "Pizza" } } };
            CreateQuestionCommand questionCommand = new CreateQuestionCommand() { question = question };

            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(questionCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }

        [TestMethod]
        public async Task Handler_shouldFail_onEmptyAnswer()
        {
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Welk fruit vind je het lekkerst?", Answers = new List<QuestionAnswer>() };
            CreateQuestionCommand questionCommand = new CreateQuestionCommand() { question = question };

            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(questionCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }
        [TestMethod]
        public async Task Handler_shouldFail_onToShortTitle()
        {
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Brood?", Answers = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "2", AnswerValue = "Pizza" } } };
            CreateQuestionCommand questionCommand = new CreateQuestionCommand() { question = question };

            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(questionCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }
        [TestMethod]
        public async Task Handler_shouldFail_onToLongTitle()
        {
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "up33RiYTr2JmKbI0rbUrrGb7LzFYyQS0pfj1WHuusZNtkSCt1S4kA5tvIE2aUaVqAq14NyVrF56RGRI8zZeAkCoQ5i8IYJNYzLeMSr5LZ5QxMi4yAW6T9If4Ed7G9Cv6jiz4lABkQutu9d74f7cs6z3eliAJuNVugcJLfSNKoMoPH7X4hHx5vyaAe7Le9kUZbZVmvjJB5bIr5ICOQvNpW1Z5m0cnlamYhHt8pWMFvVn8qqx3FvjA5ecZFz5sAMSb9ZowbNRR49vNKlgZgEXudDjELQvln9429Z4vhhPlNFqectXBkG8hJpUaelkwYcnXVXl0df9mlx8xuQhrxKD5ozcG4fDpR33YVqrtUrNXrGBYJXqbKuCWqRAVv3HSzp2iuIverkpvTq94Ut3lHBMHaqwfA7Fyzg4d2Y2I6KSbr4VvnIXvyDMZu2e3mwPYTlJTAZQQ7EXEeWqkZl477hIfdhpKgq8ZUu5btWuByIH3n4UeGCSHQIs2CyElsnWSlS2Zb\r\n", Answers = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "2", AnswerValue = "Pizza" } } };
            CreateQuestionCommand questionCommand = new CreateQuestionCommand() { question = question };

            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(questionCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }
    }
}
