using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Application.Common.Models;
using UserService.Application.Contexts.CtxUsers.Commands.CreateUser;
using UserService.Application.Contexts.Users.Commands.UpdateUser;
using UserService.Domain.Entities;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {


        private readonly IMediator _bus;


        public UserController(IMediator bus)
        {
            _bus = bus;
        }


        [HttpGet("health-check")]
        public async Task<bool> HealthCheck()
        {
            return true;
        }


        [HttpPost]
        public async Task<Result<User>> CreateUser(CreateUserCommand request)
        {
            var result = await _bus.Send(request);

            return result;
        }

        [Authorize]
        [HttpPut]
        public async Task<Result<User>> UpdateUser(UpdateUserCommand request)
        {
            request.SetUserId(GetUserIdByClaims());

            var result = await _bus.Send(request);

            return result;
        }

        [NonAction]
        public Guid GetUserIdByClaims()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Guid.Empty;

            return Guid.Parse(userId!);
        }
    }
}
