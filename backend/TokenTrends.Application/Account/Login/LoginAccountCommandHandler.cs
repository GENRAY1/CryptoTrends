using MediatR;
using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions;

namespace TokenTrends.Application.Account.Login;

public class LoginAccountCommandHandler : ICommandHandler<LoginAccountCommand>
{
    public Task<Result> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
    {
        throw new Exception("Not implemented");
    }
}