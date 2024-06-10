using DevDocs.Backend.Domain.Abstractions;
using MediatR;

namespace DevDocs.Backend.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
