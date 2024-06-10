using MediatR;
using DevDocs.Backend.Application.Interfaces;
using DevDocs.Backend.Domain.Users;
using DevDocs.Backend.Domain.Abstractions;
using DevDocs.Backend.Application.Users.Commands;
using DevDocs.Backend.Application.Abstractions;


namespace DevDocs.Backend.Application.Users.Handlers;
public class AddUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    public AddUserCommandHandler( IUserRepository userRepository, IAuthenticationService authenticationService,IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    //public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    //{
    //    var user = User.Create(new FirstName(request.FirstName), new LastName(request.LastName), new Email(request.Email));
    //    await _userRepository.AddUserAsync(user);
    //    return Result.Success(user.Id);
    //}

    //public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    //{
    //    var user = User.Create(new FirstName(request.FirstName), new LastName(request.LastName), new Email(request.Email));
    //    var keycloakUserId = await _userRepository.AddUserToKeycloakAsync(user, request.Password);
    //    user.SetIdentityId(keycloakUserId);
    //    await _userRepository.AddUserAsync(user);
    //    return Result.Success(user.Id);
    //}

    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = User.Create(
            new FirstName(request.FirstName),
            new LastName(request.LastName),
        new Email(request.Email));

        string identityId = await _authenticationService.RegisterAsync(
            user,
            request.Password,
            cancellationToken);

        user.SetIdentityId(identityId);

        await _userRepository.AddUserAsync(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

}