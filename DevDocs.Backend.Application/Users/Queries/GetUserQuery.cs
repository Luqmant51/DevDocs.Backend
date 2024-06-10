using DevDocs.Backend.Domain.Abstractions;
using MediatR;
using DevDocs.Backend.Domain.Users;

public sealed record GetUserQuery(Guid Id) : IRequest<Result<User>>;

public sealed record GetUsersQuery() : IRequest<Result<User>>;