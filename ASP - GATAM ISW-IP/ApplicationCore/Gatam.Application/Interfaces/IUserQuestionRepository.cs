using Gatam.Domain;

namespace Gatam.Application.Interfaces;

public interface IUserQuestionRepository: IGenericRepository<UserQuestion>
{
    Task<UserQuestion> GetQuestionSettingById(string id);
    Task UpdateQuestionSetting(UserQuestion setting);
}