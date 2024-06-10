using System.Net.Http.Json;
using System.Text.Json;
using DevDocs.Backend.Application.Interfaces;
using DevDocs.Backend.Domain.Users;
using DevDocs.Backend.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace DevDocs.Backend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;
    private readonly IOptions<KeycloakOptions> _keycloakOptions;


    public UserRepository(ApplicationDbContext context, HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
    {
        _context = context;
        _httpClient = httpClient;
        _keycloakOptions = keycloakOptions;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<Guid> AddUserToKeycloakAsync(User user, string password)
    {
        var payload = new
        {
            username = user.Email.Value,
            firstName = user.FirstName.Value,
            lastName = user.LastName.Value,
            email = user.Email.Value,
            enabled = true,
            credentials = new[]
            {
                new
                {
                    type = "password",
                    value = password,
                    temporary = false
                }
            }
        };

        
        Console.WriteLine($"Request Payload: {JsonSerializer.Serialize(payload)}");

        var response = await _httpClient.PostAsJsonAsync("https://identity.teamfullstack.io/admin/realms/usman/users", payload);

        Console.WriteLine($"Response Status: {response.StatusCode}");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Content: {content}");

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Failed to create user: {content}");
        }

        var keycloakUserId = JsonSerializer.Deserialize<KeycloakUserIdResponse>(content)?.Id ?? Guid.Empty;
        return keycloakUserId;
    }



    public class KeycloakUserIdResponse
    {
        public Guid Id { get; set; }
    }
}
