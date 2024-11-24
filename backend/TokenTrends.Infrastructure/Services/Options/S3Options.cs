namespace TokenTrends.Infrastructure.Services.Options;

public class S3Options
{
    public required string AccessKey { get; init; }
    public required string SecretKey { get; init; }
    public required string Endpoint { get; init; }
}