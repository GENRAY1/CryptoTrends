using TokenTrends.Application.Abstractions;

namespace TokenTrends.Application.Account.Photos.Upload;

public class UploadAccountPhotoCommand : ICommand 
{
    public required string FileName { get; init; }
    
    public required Stream FileStream { get; init; }
}