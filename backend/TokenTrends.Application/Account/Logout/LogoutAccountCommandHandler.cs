using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Account.Login;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Account.Logout;

public class LogoutAccountCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IAccountContext accountContext,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<LogoutAccountCommand>
{
    public async Task<Result> Handle(LogoutAccountCommand request, CancellationToken cancellationToken)
    {
        var accountId = accountContext.AccountId;

        if (accountId is null)
            return Result.Failure(AccountErrors.NotLoggedIn);

        var refreshToken = await refreshTokenRepository.GetByAccountId(accountId.Value, cancellationToken);
        
        if (refreshToken is null)
            return Result.Failure(AccountErrors.RefreshTokenNotFound);
        
        refreshToken.Deactivate();
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}