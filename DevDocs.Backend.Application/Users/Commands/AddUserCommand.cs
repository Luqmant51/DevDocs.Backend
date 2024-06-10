using DevDocs.Backend.Application.Abstraction.Messaging;

namespace DevDocs.Backend.Application.Users.Commands
{
    public sealed record RegisterUserCommand(
        string Email,
        string FirstName,
        string LastName,
        string Password) : ICommand<Guid>;
}
