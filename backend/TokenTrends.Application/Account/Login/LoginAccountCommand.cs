﻿using TokenTrends.Application.Abstractions;

namespace TokenTrends.Application.Account.Login;

public class LoginAccountCommand : ICommand<LoginAccountDtoResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; } 
}