using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Common.Models;
using UserService.Application.Contexts.Auth.Commands.LogIn;
using UserService.Application.Contexts.Auth.Queries.TokenOwner;
using UserService.Application.Contexts.Auth.Queries.ValidateToken;
using UserService.Domain.Common;
using UserService.Domain.Entities;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {


        private readonly IMediator _bus;


        public AuthController(IMediator bus)
        {
            _bus = bus;
        }


        [HttpGet("health-check")]
        public async Task<bool> HealthCheck()
        {
            return true;
        }

        [HttpGet("validateToken/{token}")]
        public async Task<bool> ValidateToken(string token)
        {
            var validateTokenQuery = new ValidateTokenQuery() { Token = token };

            return await _bus.Send(validateTokenQuery);
        }


        [HttpGet("tokenOwner/{token}")]
        public async Task<Result<User>> TokenOwner(string token)
        {
            var getTokenOwnerQuery = new GetTokenOwnerQuery() { Token = token };

            return await _bus.Send(getTokenOwnerQuery);
        }


        [HttpPost("login")]
        public async Task<Result<RefreshToken>> CreateUser(LogInUserCommand request)
        {
            var result = await _bus.Send(request);

            return result;
        }
    }
}
