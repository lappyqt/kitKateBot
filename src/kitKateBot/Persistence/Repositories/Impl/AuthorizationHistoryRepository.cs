
using System.Linq.Expressions;
using kitKateBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace kitKateBot.Persistence.Repositories.Impl;

public class AuthorizationHistoryRepository : IAuthorizationHistoryRepository
{
    private readonly ApplicationContext _context;

    public AuthorizationHistoryRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<AuthorizationHistory?> GetAsync(Expression<Func<AuthorizationHistory, bool>> predicate)
    {
        return await _context.AuthorizationHistory.OrderByDescending(x => x.AuthorizedAt).FirstOrDefaultAsync(predicate);
    }

    public async Task<List<AuthorizationHistory>> GetAllAsync(Expression<Func<AuthorizationHistory, bool>> predicate)
    {
        return await _context.AuthorizationHistory.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(AuthorizationHistory authorizationHistory)
    {
        await _context.AuthorizationHistory.AddAsync(authorizationHistory);
        await _context.SaveChangesAsync();
    }
}