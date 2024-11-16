using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Account.Get;

public class GetAccountQueryHandler(
    IAccountContext accountContext,
    IAccountRepository accountRepository
    ) : IQueryHandler<GetAccountQuery, GetAccountDtoResponse>
{
    public async Task<Result<GetAccountDtoResponse>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var accountId = accountContext.AccountId;
        
        if(accountId is null)
            return Result.Failure<GetAccountDtoResponse>(AccountErrors.NotLoggedIn); 

        var account = await accountRepository
            .GetById(accountId.Value, cancellationToken);
        
        if(account is null)
            return Result.Failure<GetAccountDtoResponse>(AccountErrors.AccountNotFound);
        
        return Result.Success(new GetAccountDtoResponse
        {
            Email = account.Email,
            Username = account.Username,
            Roles = account.Roles
                .Select(r => r.Name)
                .ToList()
        });
        
    }
}