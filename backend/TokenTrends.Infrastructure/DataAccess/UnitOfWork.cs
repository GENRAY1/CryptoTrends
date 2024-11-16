using TokenTrends.Domain.Absractions;

namespace TokenTrends.Infrastructure.DataAccess;

public class UnitOfWork(ApplicationDbContext context)
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}