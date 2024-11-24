using Minio;
using Minio.DataModel.Args;
using TokenTrends.Application.Services.FileServices;
using TokenTrends.Infrastructure.Services.FileService.Exceptions;
using Bucket = Minio.DataModel.Bucket;

namespace TokenTrends.Infrastructure.Services.FileService;

public class FileService(IMinioClient minioClient) : IFileService
{
    public async Task<string> UploadFileAsync(
        FileUploadType type,
        string fileName,
        Stream stream,
        CancellationToken cancellationToken)
    {
        string extension = Path.GetExtension(fileName);

        if (!type.Extensions.Contains(extension))
            throw new UnsupportedFileExtensionException(extension);

        if (type.MaxSize < stream.Length)
            throw new FileSizeExceededException();
        
        var objectName = $"{type.Name}_{Guid.NewGuid()}{extension}";

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(type.Bucket)
            .WithObject(objectName)
            .WithObjectSize(stream.Length)
            .WithStreamData(stream);

        await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
        
        return objectName;
    }

    public async Task<DownloadedFile> DownloadFileAsync(
        string bucket,
        string fileName, 
        CancellationToken cancellationToken)
    {
        Stream fileStream = new MemoryStream();

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(fileStream);
                fileStream.Position = 0; 
                stream.Dispose();
            });
        
        var response = await minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
        
        return new DownloadedFile
        {
            ContentType = response.ContentType,
            FileName = fileName,
            Stream = fileStream
        };
    }

    public async Task DeleteFileAsync(
        string bucket,
        string fileName,
        CancellationToken cancellationToken)
    {
        var deleteObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName);
        
        await minioClient.RemoveObjectAsync(deleteObjectArgs, cancellationToken);
    }
}