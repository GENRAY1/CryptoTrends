using MediatR;
using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}