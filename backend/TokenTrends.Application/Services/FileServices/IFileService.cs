namespace TokenTrends.Application.Services.FileServices;

public interface IFileService
{
    Task<string> UploadFileAsync(
        FileUploadType fileUploadType, 
        string fileName, 
        Stream stream,
        CancellationToken cancellationToken);

    Task<DownloadedFile> DownloadFileAsync(
        string bucket,
        string fileName,
        CancellationToken cancellationToken);

    Task DeleteFileAsync(
        string bucket,
        string fileName,
        CancellationToken cancellationToken);
}