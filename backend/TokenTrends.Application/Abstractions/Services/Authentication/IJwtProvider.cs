namespace TokenTrends.Application.Abstractions.Services.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(AccountDto account);
    string GenerateRefreshToken();
    int GetRefreshTokenLifetimeInMinutes();
    Guid? GetAccountIdByToken(string token);
}