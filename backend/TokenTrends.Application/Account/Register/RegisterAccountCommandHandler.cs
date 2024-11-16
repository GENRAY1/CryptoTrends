using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Account.Register;

public class RegisterAccountCommandHandler(
    IAccountRepository accountRepository,
    IPasswordManager passwordManager,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterAccountCommand>
{
    public async Task<Result> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = passwordManager.Generate(request.Password);
        
        var existingAccount = await accountRepository.GetByEmail(request.Email, cancellationToken);
        
        if (existingAccount is not null)
            return Result.Failure(AccountErrors.EmailAlreadyExists);
        
        var account = Domain.Account.Account.Create(
            request.Email,
            passwordHash,
            request.Username);

        accountRepository.Add(account);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}