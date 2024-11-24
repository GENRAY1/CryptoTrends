using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Services.Authentication;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Account.RefreshToken;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IAccountRepository accountRepository,
    IJwtProvider jwtProvider,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        Guid? accountId = jwtProvider.GetAccountIdByToken(request.AccessToken);
        
        if (accountId is null)
            return Result.Failure<RefreshTokenResponse>(AccountErrors.AccessTokenInvalid);
        
        var refreshToken = await refreshTokenRepository.GetByAccountId(accountId.Value, cancellationToken);

        if (refreshToken is null)
            return Result.Failure<RefreshTokenResponse>(AccountErrors.RefreshTokenNotFound);

        var dateNow = DateTime.UtcNow;
        
        var validate  = refreshToken.Validate(dateNow, request.RefreshToken);

        if (validate.IsFailure)
            return Result.Failure<RefreshTokenResponse>(validate.Error);
        
        var account = await accountRepository.GetById(accountId.Value, cancellationToken);
        
        if (account is null)
            return Result.Failure<RefreshTokenResponse>(AccountErrors.AccountNotFound);
        
        var accessTokenValue = jwtProvider.GenerateAccessToken(new AccountDto
        {
            Email = account.Email,
            Id = account.Id,
            Roles = account.Roles.Select(e => e.Name).ToList(),
        });
        
        var refreshTokenValue = jwtProvider.GenerateRefreshToken();
        
        int lifetimeInMinutes = jwtProvider.GetRefreshTokenLifetimeInMinutes();
        
        var expirationDate = DateTime.UtcNow.AddMinutes(lifetimeInMinutes);

        refreshToken.Activate(refreshTokenValue, expirationDate);
        
        refreshTokenRepository.Update(refreshToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new RefreshTokenResponse
        {
            AccessToken = accessTokenValue,
            RefreshToken = refreshTokenValue
        });
    }
}