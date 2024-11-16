namespace TokenTrends.Presentation.Controllers.Accounts;

public class RegisterAccountRequest
{
    public required string Email { get; init; }
    
    public required string Password { get; init; }
    
    public required string Username { get; init; }
}