using Microsoft.EntityFrameworkCore;
using TokenTrends.Domain.Account.Identity;
using TokenTrends.Domain.Account.Identity.Enums;

namespace TokenTrends.Infrastructure.DataAccess.Repositories;

public class ConfirmationCodeRepository(ApplicationDbContext context) : IConfirmationCodeRepository
{
    public void Add(ConfirmationCode code)
    {
        context.Add(code);
    }
    
    public async Task<int> GetCountFromLastHour(Guid accountId, ConfirmationCodeEvent codeEvent)
    {
        return await context
            .Set<ConfirmationCode>()
            .Where(x =>
                x.AccountId == accountId 
                && x.Event == codeEvent
                && x.CreatedAt > DateTime.UtcNow.AddHours(-1))
            .CountAsync();
    }

    public async Task<ConfirmationCode?> GetLatestByAccountId(
        Guid accountId, 
        ConfirmationCodeEvent codeEvent, 
        CancellationToken cancellationToken)
    {
        return await context
            .Set<ConfirmationCode>()
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken); 
    }

    public async Task<ConfirmationCode?> GetByCode(
        string code, 
        CancellationToken cancellationToken)
    {
        return await context
            .Set<ConfirmationCode>()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public void Update(ConfirmationCode code)
    {
         context.Update(code);
    }
}