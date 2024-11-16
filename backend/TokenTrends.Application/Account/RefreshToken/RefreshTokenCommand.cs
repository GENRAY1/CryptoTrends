using TokenTrends.Application.Abstractions;

namespace TokenTrends.Application.Account.RefreshToken;

public class RefreshTokenCommand : ICommand<RefreshTokenResponse>
{
    public required string RefreshToken { get; init; }
    public required string AccessToken { get; set; }
}