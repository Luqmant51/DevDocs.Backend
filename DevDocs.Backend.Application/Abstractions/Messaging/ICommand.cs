
using DevDocs.Backend.Domain.Abstractions;
using MediatR;

namespace DevDocs.Backend.Application.Abstraction.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TReponse> : IRequest<Result<TReponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}
