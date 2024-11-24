namespace TokenTrends.Application.Services.Authentication;

public class AccountDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required IReadOnlyCollection<string> Roles { get; set; }
}