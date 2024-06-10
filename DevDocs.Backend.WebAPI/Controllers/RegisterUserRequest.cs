namespace DevDocs.Backend.WebAPI.Controllers;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);
