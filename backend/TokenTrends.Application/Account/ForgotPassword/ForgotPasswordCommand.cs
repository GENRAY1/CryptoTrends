using TokenTrends.Application.Abstractions;

namespace TokenTrends.Application.Account.ForgotPassword;

public class ForgotPasswordCommand : ICommand
{
    public required string Email { get; init; }
}