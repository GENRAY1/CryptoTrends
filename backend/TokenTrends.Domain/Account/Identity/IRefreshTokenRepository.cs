namespace TokenTrends.Domain.Account.Identity;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
    
    void Add(RefreshToken refreshToken);
    
    void Update(RefreshToken refreshToken);
}