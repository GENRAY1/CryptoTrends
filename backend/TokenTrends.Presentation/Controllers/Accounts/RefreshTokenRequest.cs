namespace TokenTrends.Presentation.Controllers.Accounts;

public class RefreshTokenRequest
{
    public required string RefreshToken { get; init; }
    
    public required string AccessToken { get; set; }
}