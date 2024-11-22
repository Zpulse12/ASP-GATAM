using Gatam.Domain;

namespace Gatam.Application.Interfaces;

public interface IUserModuleQuestionSettingRepository: IGenericRepository<UserModuleQuestionSetting>
{
    Task<UserModuleQuestionSetting> GetQuestionSettingById(string id);
    Task UpdateQuestionSetting(UserModuleQuestionSetting setting);
}