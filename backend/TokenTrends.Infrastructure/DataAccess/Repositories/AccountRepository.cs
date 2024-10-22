using Microsoft.EntityFrameworkCore;
using TokenTrends.Domain.Account;

namespace TokenTrends.Infrastructure.DataAccess.Repositories;

public class AccountRepository(ApplicationDbContext context)
    : IAccountRepository
{
    public async Task<Account?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return await context
            .Set<Account>()
            .Include(account => account.Roles)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<Account?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context
            .Set<Account>()
            .Include(account => account.Roles)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(Account account)
    {
        if (account.Roles.Count > 0)
        {
            context.AttachRange(account.Roles);
        }
        
        context.Add(account);
    }

    public void Update(Account account)
    {
        context.Update(account);
    }
}