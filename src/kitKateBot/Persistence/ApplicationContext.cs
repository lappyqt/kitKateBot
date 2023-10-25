using kitKateBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace kitKateBot.Persistence;

public class ApplicationContext : DbContext
{
    public DbSet<AuthorizationHistory> AuthorizationHistory { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
    {
        Database.EnsureCreated();
    }
}