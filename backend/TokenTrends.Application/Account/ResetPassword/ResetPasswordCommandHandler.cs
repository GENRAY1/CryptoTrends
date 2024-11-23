using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;
using TokenTrends.Domain.Account.Identity.Enums;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Account.ResetPassword;

public class ResetPasswordCommandHandler(
    IConfirmationCodeRepository confirmationCodeRepository,
    IAccountRepository accountRepository,
    IPasswordManager passwordManager,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ResetPasswordCommand, ResetPasswordDtoResponse>
{
    public async Task<Result<ResetPasswordDtoResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var confirmationCode = 
            await confirmationCodeRepository.GetByCode(request.Code, cancellationToken); 
        
        if (confirmationCode is null)
            return Result.Failure<ResetPasswordDtoResponse>(new Error("AccountErrors.ConfirmationCodeNotFound","Confirmation code not found"));

        if (confirmationCode.Status != ConfirmationCodeStatus.Pending)
            return Result.Failure<ResetPasswordDtoResponse>(new Error("AccountErrors.ConfirmationCodeAlreadyUsed","Confirmation code already used"));

        if (request.NewPassword is null)
        {
            return Result.Success(new ResetPasswordDtoResponse
            {
                CodeValid = true
            });
        }
        
        var account = await accountRepository.GetById(confirmationCode.AccountId, cancellationToken);
        
        if(account is null)
            return Result.Failure<ResetPasswordDtoResponse>(AccountErrors.AccountNotFound);
        
        var passwordHash = passwordManager.Generate(request.NewPassword);
        
        account.ChangePassword(passwordHash);
        
        confirmationCode.Confirm();
        
        accountRepository.Update(account);
        
        confirmationCodeRepository.Update(confirmationCode);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(new ResetPasswordDtoResponse
        {
            CodeValid = true,
            PasswordChanged = true
        });
    }
}