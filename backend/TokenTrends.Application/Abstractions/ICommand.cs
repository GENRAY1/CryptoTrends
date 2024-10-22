using MediatR;
using PetPalsProfile.Domain.Absractions;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Abstractions;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
