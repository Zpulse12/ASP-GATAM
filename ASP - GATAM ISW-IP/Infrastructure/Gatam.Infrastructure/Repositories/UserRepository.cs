using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Gatam.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetUserWithModules(string userId)
    {
        return await _context.Users
            .Include(u => u.UserModules)
            .ThenInclude(um => um.Module)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
    public async Task<List<ApplicationUser>> GetUsersWithModulesAsync()
    {
        return await _context.Users
            .Include(u => u.UserModules)
                .ThenInclude(um => um.Module)
                    .ThenInclude(m => m.Questions)
                    .ThenInclude(m => m.Answers)
            .Include(u => u.UserModules)
                .ThenInclude(um => um.UserGivenAnswers)
            .Include(u => u.UserModules)
                .ThenInclude(um => um.UserQuestions)
                    .ThenInclude(qs => qs.Question) 
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<ApplicationUser>> GetUsersForBegeleiderAsync(string begeleiderId)
    {
        return await _context.Users
        .Where(u => u.BegeleiderId == begeleiderId)
        .ToListAsync();
    }
    public void RemoveUserRole(UserRole userRole)
    {
        _context.UserRoles.Remove(userRole);
    }

    public async Task<ApplicationUser?> GetUserWithRoles(string userId)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}