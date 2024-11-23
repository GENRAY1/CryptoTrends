using TokenTrends.Application.Abstractions;

namespace TokenTrends.Application.Account.ResetPassword;

public class ResetPasswordCommand : ICommand<ResetPasswordDtoResponse>
{
    public required string Code { get; init; }
    
    public string? NewPassword { get; init; }
}