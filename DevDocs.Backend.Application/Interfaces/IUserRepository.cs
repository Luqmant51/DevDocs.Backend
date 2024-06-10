using DevDocs.Backend.Domain.Users;

namespace DevDocs.Backend.Application.Interfaces;

    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserAsync(Guid id);
        Task<Guid> AddUserToKeycloakAsync(User user, string password);
    }
