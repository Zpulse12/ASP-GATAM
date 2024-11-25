using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Gatam.Infrastructure.Repositories;

public class UserQuestionRepository: GenericRepository<UserQuestion>, IUserQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public UserQuestionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserQuestion> GetQuestionSettingById(string id)
    {
        return await _context.UserQuestion
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}