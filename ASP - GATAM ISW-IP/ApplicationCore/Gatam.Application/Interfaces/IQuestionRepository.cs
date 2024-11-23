using Gatam.Domain;

namespace Gatam.Application.Interfaces;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<Question> GetQuestionAndAnswers(string Id);
    Task UpdateQuestionAndAnswers(Question entity);
    Task<List<Question>> GetQuestionsByModuleIdAsync(string moduleId, bool includeAnswers = false);
}
