namespace TokenTrends.Domain.Account.Identity;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
    
    void Add(RefreshToken account);
    
    void Update(RefreshToken account);
}