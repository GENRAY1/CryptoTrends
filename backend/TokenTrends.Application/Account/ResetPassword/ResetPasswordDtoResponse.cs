namespace TokenTrends.Application.Account.ResetPassword;

public class ResetPasswordDtoResponse
{
    public bool CodeValid { get; init; }
    
    public bool PasswordChanged { get; init; }
}