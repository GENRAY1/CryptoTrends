namespace TokenTrends.Presentation.Controllers.Accounts;

public class LoginAccountRequest
{
    public required string Email { get; set; }
    
    public required string Password { get; set; } 
}