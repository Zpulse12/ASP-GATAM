using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Domain;

namespace Gatam.Application.CQRS.DTOS.ModulesDTO;

public class ModuleDTO
{
    public string Id { get; set; }
    public  string Title { get; set; }
    public  string Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();
}