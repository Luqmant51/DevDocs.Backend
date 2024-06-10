using DevDocs.Backend.Domain.Abstractions;
using MediatR;

namespace DevDocs.Backend.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
