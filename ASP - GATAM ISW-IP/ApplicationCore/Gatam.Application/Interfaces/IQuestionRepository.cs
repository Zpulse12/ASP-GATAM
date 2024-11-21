using Gatam.Domain;

namespace Gatam.Application.Interfaces;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<Question> GetQuestionAndAnswers(string Id);
    Task UpdateQuestionAndAnswers(Question entity);
    Task<List<Question>> GetVisibleQuestionsForVolger(string volgerId);
    Task<UserModuleQuestionSetting> GetQuestionSettingById(string id);
    Task UpdateQuestionSetting(UserModuleQuestionSetting setting);
}
