using Gatam.Application.CQRS;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Domain;

public class UserModuleDTO
{
    public string Id { get; set; }
    public UserDTO User { get; set; }
    public ModuleDTO Module { get; set; } 
    public List<UserQuestionDTO> UserQuestion { get; set; }
}
