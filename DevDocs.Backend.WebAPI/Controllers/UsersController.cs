using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevDocs.Backend.Application.Users.Commands;
using DevDocs.Backend.Domain.Abstractions;

namespace DevDocs.Backend.WebAPI.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("user")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password);

            Result<Guid> result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var query = new GetUserQuery(id);
            var result = await _sender.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Error);
        }
    }
}
