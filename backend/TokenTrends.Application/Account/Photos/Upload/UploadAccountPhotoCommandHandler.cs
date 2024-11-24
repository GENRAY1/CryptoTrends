using TokenTrends.Application.Abstractions;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Services.Authentication;
using TokenTrends.Application.Services.FileServices;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Account.Photos.Upload;

public class UploadAccountPhotoCommandHandler(
    IFileService fileService,
    IAccountRepository accountRepository,
    IAccountContext accountContext,
    IUnitOfWork unitOfWork) : ICommandHandler<UploadAccountPhotoCommand>
{
    public async Task<Result> Handle(UploadAccountPhotoCommand request, CancellationToken cancellationToken)
    {
        Guid accountId = accountContext.AccountId;

        var account = await accountRepository.GetById(accountId, cancellationToken);

        if (account is null)
            return Result.Failure(AccountErrors.AccountNotFound);

        if (account.PhotoFileName is not null)
        {
            await fileService.DeleteFileAsync(Buckets.Account, account.PhotoFileName, cancellationToken);
        }
        
        var accountPhoto = new FileUploadType
        {
            Bucket = Buckets.Account,
            Name = "accountPhoto",
            Extensions = new[] { ".jpg", ".jpeg", ".png" },
            MaxSize = 10 * 1024 * 1024
        };

        var fileName = await fileService.UploadFileAsync(accountPhoto, request.FileName, request.FileStream, cancellationToken);
        
        account.SetPhoto(fileName);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}