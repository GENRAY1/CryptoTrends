namespace TokenTrends.Application.Services.FileServices;

public class DownloadedFile
{
    public required string FileName { get; init; } 
    public required string ContentType { get; init; }
    public required Stream Stream { get; init; }
}