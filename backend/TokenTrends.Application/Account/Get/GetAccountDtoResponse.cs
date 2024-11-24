namespace TokenTrends.Application.Account.Get;

public class GetAccountDtoResponse
{
    public required string Email { get; init; }

    public required string Username { get; init; }
    
    public string? PhotoFileName { get; init; }
    public required IReadOnlyCollection<string> Roles { get; set; }
}