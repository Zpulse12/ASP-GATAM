using Gatam.Domain;

namespace Gatam.Application.CQRS.DTOS.QuestionsDTO;

public class QuestionSettingDTO
{
    public string Id { get; set; }
    public bool IsVisible { get; set; }
    public string QuestionId { get; set; }
    public string QuestionTitle { get; set; }
    public int QuestionType { get; set; }
}
