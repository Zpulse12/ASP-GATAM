using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Domain;

namespace Gatam.Application.CQRS.DTOS.ModulesDTO
{
public class UserModuleDTO
{
    public string Id { get; set; }
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string? UserPicture { get; set; }
        public string? UserPhoneNumber{ get; set; }
        // public UserDTO User { get; set; }
        public string ModuleId { get; set; }
        public ModuleDTO Module { get; set; } 
        public UserModuleState State { get; set; }
        public ICollection<UserQuestionDTO> UserQuestions { get; set; }
        public ICollection<UserAnswerDTO> UserGivenAnswers { get; set; }
        public UserModuleDTO()
        {
            Id = Guid.NewGuid().ToString();
            State = UserModuleState.NotStarted;
        }
    }
}
