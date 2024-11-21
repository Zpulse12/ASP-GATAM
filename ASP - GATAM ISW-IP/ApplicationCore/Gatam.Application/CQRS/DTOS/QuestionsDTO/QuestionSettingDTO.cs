using Gatam.Domain;

namespace Gatam.Application.CQRS.DTOS.QuestionsDTO;

public class QuestionSettingDTO
{
    public string Id { get; set; }
    public bool IsVisible { get; set; }
    public QuestionDTO Question { get; set; }
}
