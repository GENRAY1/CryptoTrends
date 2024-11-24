using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Account.Login;
using TokenTrends.Application.Services.Authentication;
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
        
        var refreshToken = await refreshTokenRepository.GetByAccountId(accountId, cancellationToken);
        
        if (refreshToken is null)
            return Result.Failure(AccountErrors.RefreshTokenNotFound);
        
        refreshToken.Deactivate();
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}