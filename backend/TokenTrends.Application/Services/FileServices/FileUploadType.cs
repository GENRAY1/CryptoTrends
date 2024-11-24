namespace TokenTrends.Application.Services.FileServices;

public class FileUploadType
{
    public required string Name { get; init; }
    public required string[] Extensions { get; init; }
    public required string Bucket { get; init; }
    public required long MaxSize { get; init; }
}