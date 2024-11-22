using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Gatam.Infrastructure.Repositories;

public class UserModuleQuestionSettingRepository: GenericRepository<UserModuleQuestionSetting>, IUserModuleQuestionSettingRepository
{
    private readonly ApplicationDbContext _context;

    public UserModuleQuestionSettingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserModuleQuestionSetting> GetQuestionSettingById(string id)
    {
        return await _context.UserModuleQuestionSetting
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateQuestionSetting(UserModuleQuestionSetting setting)
    {
        _context.UserModuleQuestionSetting.Update(setting);
        await _context.SaveChangesAsync();
    }
}