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
            .AsNoTracking()
            .ToListAsync();
    }
}