using MediatR;
using DevDocs.Backend.Application.Interfaces;
using DevDocs.Backend.Domain.Users;
using DevDocs.Backend.Domain.Abstractions;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.Id);
        if (user == null)
        {
            return Result.Failure<User>(new Error("UserNotFound", "User not found"));
        }
        return Result.Success(user);
    }
}