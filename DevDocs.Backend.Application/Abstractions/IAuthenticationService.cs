using DevDocs.Backend.Domain.Users;

namespace DevDocs.Backend.Application.Abstractions;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default);
}
