using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Domain;

namespace Gatam.Application.CQRS.DTOS.ModulesDTO
{
public class UserModuleDTO
{
    public string Id { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public ModuleDTO Module { get; set; } 
        public UserModuleState State { get; set; }
        public List<UserQuestionDTO> UserQuestions { get; set; }
        public List<UserAnswer> UserGivenAnswers { get; set; } 
    }
}
