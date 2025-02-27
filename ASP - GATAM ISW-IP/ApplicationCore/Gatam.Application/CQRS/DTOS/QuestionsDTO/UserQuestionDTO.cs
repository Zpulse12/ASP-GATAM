using Gatam.Domain;

namespace Gatam.Application.CQRS.DTOS.QuestionsDTO;

public class UserQuestionDTO
{
    public string Id { get; set; }
    public string UserModuleId { get; set; }
    public string QuestionId { get; set; }
    public string QuestionTitle { get; set; }
    public int QuestionType { get; set; }
    public bool IsVisible { get; set; }
    public QuestionPriority QuestionPriority { get; set; }
    public IEnumerable<QuestionAnswerDTO> Answers { get; set; } = new List<QuestionAnswerDTO>();
    public UserQuestionDTO()
    {
        Id = Guid.NewGuid().ToString();
        IsVisible = true;

    }

}
