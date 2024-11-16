using MediatR;
using PetPalsProfile.Domain.Absractions;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
