using Microsoft.EntityFrameworkCore;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;

namespace TokenTrends.Infrastructure.DataAccess.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) 
    : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByAccountId(Guid accountId, CancellationToken cancellationToken)
    {
        return await context
            .Set<RefreshToken>()
            .FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);
    }

    public void Add(RefreshToken refreshToken)
    {
        context.Add(refreshToken);
    }

    public void Update(RefreshToken refreshToken)
    {
        context.Update(refreshToken);
    }
}