using DevDocs.Backend.Domain.Abstractions;

namespace DevDocs.Backend.Application.Abstractions;

public interface IJwtService
{
    Task<Result<string>> GetAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}
