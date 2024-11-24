using TokenTrends.Application.Abstractions;

namespace TokenTrends.Application.Account.Photos.Delete;

public class DeleteAccountPhotoCommand : ICommand
{
    public required string FileName { get; init; }
}