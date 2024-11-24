using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Services.Email;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;
using TokenTrends.Domain.Account.Identity.Enums;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Account.ForgotPassword;

public class ForgotPasswordCommandHandler(
    IEmailService emailService,
    IConfirmationCodeRepository confirmationCodeRepository,
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ForgotPasswordCommand>
{
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByEmail(request.Email, cancellationToken);

        if (account is null)
            return Result.Failure(AccountErrors.EmailNotFound);

        var countAttemptsFromLastHour =
            await confirmationCodeRepository.GetCountFromLastHour(account.Id, ConfirmationCodeEvent.ResetPassword);

        if (countAttemptsFromLastHour != 0)
        {
            var latestConfirmationCode =
                await confirmationCodeRepository.GetLatestByAccountId(account.Id, ConfirmationCodeEvent.ResetPassword,
                    cancellationToken);

            if (latestConfirmationCode is not null && latestConfirmationCode.Status == ConfirmationCodeStatus.Pending)
            {
                latestConfirmationCode.Fail();

                confirmationCodeRepository.Update(latestConfirmationCode);
            }
        }

        if (ConfirmationCode.MaxAttemptCount <= countAttemptsFromLastHour)
            return Result.Failure(AccountErrors.TooManyAttempts);

        var confirmationCode = ConfirmationCode.Create(account.Id, ConfirmationCodeEvent.ResetPassword);

        await emailService.SendEmailAsync(
            account.Email,
            "TokenTrends - Reset Password",
            $"""
             Dear {account.Username},

             We have received a request to reset the password for your account. If you did not make this request, please ignore this email.

             To complete the password reset process, please use the following confirmation code:

             Confirmation Code: {confirmationCode.Code}

             This code is valid for [duration, e.g., 1 hour]. Please do not share this code with anyone.

             Best regards,
             TokenTrends
             """);


        confirmationCodeRepository.Add(confirmationCode);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}