using System.Linq.Expressions;
using kitKateBot.Domain.Entities;

namespace kitKateBot.Persistence.Repositories.Impl;

public interface IAuthorizationHistoryRepository
{
    Task<AuthorizationHistory?> GetAsync(Expression<Func<AuthorizationHistory, bool>> predicate);
    Task<List<AuthorizationHistory>> GetAllAsync(Expression<Func<AuthorizationHistory, bool>> predicate);
    Task AddAsync(AuthorizationHistory authorizationHistory);
}