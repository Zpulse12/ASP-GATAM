//using FluentValidation;
//using Gatam.Application.CQRS.Module;
//using Gatam.Domain;

//using Gatam.Application.Interfaces;
//using Moq;

//namespace UnitTesting.CQRSTest.ApplicationModule
//{
//    [TestClass]
//    public class CreateModuleCommandTest
//    {
//        private Mock<IUnitOfWork> _unitOfWorkMock;
//        private CreateModuleCommandHandler _handler;

//        [TestInitialize]
//        public void Setup()
//        {
//            _unitOfWorkMock = new Mock<IUnitOfWork>();
//            _handler = new CreateModuleCommandHandler(_unitOfWorkMock.Object);
//        }

//        [TestMethod]
//        public async Task Handle_ShouldCreateModuleAndQuestions_WhenCommandIsValid()
//        {
//            // Gegeven: een module en vragen
//            var module = new Gatam.Domain.ApplicationModule
//            {
//                Title = "Test Module",
//                Category = "Test Category",
//                Questions = new List<Gatam.Domain.Question>() 
//            };

//            var questions = new List<Gatam.Domain.Question>
//            {
//                new Gatam.Domain.Question { QuestionTitle = "Question 1" },
//                new Gatam.Domain.Question { QuestionTitle = "Question 2" }
//            };

//            var command = new CreateModuleCommand { _module = module, question = questions };

//            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.Create(module)).ReturnsAsync(module);
//            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.CompletedTask);

//            _unitOfWorkMock.Setup(uow => uow.QuestionRepository.Create(It.IsAny<Gatam.Domain.Question>()));

//            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.GetModuleWithQuestionsAndAnswersAsync(module.Id))
//                .ReturnsAsync(module); 

//            //Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            _unitOfWorkMock.Verify(uow => uow.ModuleRepository.Create(module), Times.Once);
//            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Exactly(2)); 

//            foreach (var question in questions)
//            {
//                Assert.IsTrue(result.Questions.Contains(question)); 
//            }

//            //Assert
//            Assert.AreEqual(module, result);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ValidationException))]
//        public async Task Handle_ShouldThrowValidationException_WhenModuleIsInvalid()
//        {
//            var module = new Gatam.Domain.ApplicationModule { Title = "", Category = "Test Category" }; // Ongeldige data
//            var command = new CreateModuleCommand { _module = module };

//            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.Create(It.IsAny<Gatam.Domain.ApplicationModule>()))
//                .ThrowsAsync(new ValidationException("Module validation failed"));

//            await _handler.Handle(command, CancellationToken.None);
//        }

//        [TestMethod]
//        public async Task Handle_ShouldReturnModuleWithQuestions_WhenCommitIsSuccessful()
//        {
//            var module = new Gatam.Domain.ApplicationModule { Title = "Test Module", Category = "Test Category" };
//            var questions = new List<Gatam.Domain.Question>
//            {
//                new Gatam.Domain.Question { QuestionTitle = "Question 1" },
//                new Gatam.Domain.Question { QuestionTitle = "Question 2" }
//            };

//            var command = new CreateModuleCommand { _module = module, question = questions };

//            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.Create(module)).ReturnsAsync(module);
//            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.CompletedTask);

//            _unitOfWorkMock.Setup(uow => uow.QuestionRepository.Create(It.IsAny<Gatam.Domain.Question>()));

//            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.GetModuleWithQuestionsAndAnswersAsync(module.Id))
//                .ReturnsAsync(module); 

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            Assert.AreEqual(module.Title, result.Title);
//            Assert.AreEqual(module.Category, result.Category);
//            Assert.AreEqual(questions.Count, result.Questions.Count);
//            foreach (var question in questions)
//            {
//                Assert.IsTrue(result.Questions.Contains(question)); 
//            }
//        }
//    }
//}
