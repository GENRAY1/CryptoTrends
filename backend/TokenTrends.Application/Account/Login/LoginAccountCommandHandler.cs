using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Services.Authentication;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;
using TokenTrends.Domain.Common;
using static TokenTrends.Domain.Account.Identity.RefreshToken;

namespace TokenTrends.Application.Account.Login;

public class LoginAccountCommandHandler(
    IAccountRepository accountRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordManager passwordManager, 
    IJwtProvider jwtProvider,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<LoginAccountCommand, LoginAccountDtoResponse>
{
    public async Task<Result<LoginAccountDtoResponse>> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByEmail(request.Email, cancellationToken);
        
        if (account is null || !passwordManager.Verify(request.Password, account.Password))
            return Result.Failure<LoginAccountDtoResponse>(AccountErrors.InvalidCredentials);
        
        var accessToken = jwtProvider.GenerateAccessToken(
            new AccountDto
            {
                Email = account.Email,
                Id = account.Id,
                Roles = account.Roles.Select(r => r.Name).ToList()
            });
        
        var refreshTokenValue = jwtProvider.GenerateRefreshToken();

        int lifetimeInMinutes = jwtProvider.GetRefreshTokenLifetimeInMinutes();
        
        var expirationDate = DateTime.UtcNow.AddMinutes(lifetimeInMinutes);

        var refreshToken = await refreshTokenRepository.GetByAccountId(account.Id, cancellationToken);

        if (refreshToken is not null)
        {
            refreshToken.Activate(refreshTokenValue, expirationDate);
            
            refreshTokenRepository.Update(refreshToken);
        }
        else
        {
            var newRefreshToken = Create(account.Id, refreshTokenValue, expirationDate);
            
            refreshTokenRepository.Add(newRefreshToken);
        }
        
        account.Logged();
        accountRepository.Update(account);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new LoginAccountDtoResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
    }
}