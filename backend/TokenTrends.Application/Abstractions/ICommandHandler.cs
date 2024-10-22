using MediatR;
using PetPalsProfile.Domain.Absractions;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Abstractions;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}