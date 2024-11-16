namespace TokenTrends.Application.Account.RefreshToken;

public class RefreshTokenResponse
{
    public required string AccessToken { get; init; }
    
    public required string RefreshToken { get; init; }
}