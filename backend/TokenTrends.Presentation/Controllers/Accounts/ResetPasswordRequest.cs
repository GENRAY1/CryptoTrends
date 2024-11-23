namespace TokenTrends.Presentation.Controllers.Accounts;

public class ResetPasswordRequest
{
    public required string Code { get; init; }
    
    public string? NewPassword { get; init; }
}