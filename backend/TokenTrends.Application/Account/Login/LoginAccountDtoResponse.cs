namespace TokenTrends.Application.Account.Login;

public class LoginAccountDtoResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}