using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Services.Authentication;
using TokenTrends.Application.Services.FileServices;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Common;
using TokenTrends.Domain.Files;

namespace TokenTrends.Application.Account.Photos.Delete;

public class DeleteAccountPhotoCommandHandler(
    IAccountRepository accountRepository,
    IFileService fileService,
    IAccountContext accountContext,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteAccountPhotoCommand>
{
    public async Task<Result> Handle(DeleteAccountPhotoCommand request, CancellationToken cancellationToken)
    {
        Guid accountId = accountContext.AccountId;
        
        var account = await accountRepository.GetById(accountId, cancellationToken);

        if (account is null)
            return Result.Failure(AccountErrors.AccountNotFound);

        if (account.PhotoFileName != request.FileName)
            return Result.Failure(FileErrors.FileNotFound);
        
        await fileService.DeleteFileAsync(Buckets.Account, account.PhotoFileName, cancellationToken);
        
        account.RemovePhoto();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}