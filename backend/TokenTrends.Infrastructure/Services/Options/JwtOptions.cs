namespace TokenTrends.Infrastructure.Services.Options;

public class JwtOptions
{
    public required AccessTokenSettings AccessTokenSettings { get; init; }
    public required RefreshTokenSettings RefreshTokenSettings { get; init; }
}

public class AccessTokenSettings
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required long LifeTimeInMinutes { get; set; }
    public required string Key { get; set; }
}

public class RefreshTokenSettings
{
    public required int Length { get; init; }
    public required int LifeTimeInMinutes { get; init; }
}