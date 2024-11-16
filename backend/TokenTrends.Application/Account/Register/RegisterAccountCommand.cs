using TokenTrends.Application.Abstractions;

namespace TokenTrends.Application.Account.Register;

public class RegisterAccountCommand : ICommand
{
    public required string Email { get; set; }
    public required string Password { get; set; } 
    public required string Username { get; set; }
}