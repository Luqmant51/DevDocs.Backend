using DevDocs.Backend.Application.Abstraction.Messaging;
using DevDocs.Backend.Domain.Abstractions;
using MediatR;

namespace DevDocs.Backend.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
