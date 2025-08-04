using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Common.Models;
using UserService.Application.Contexts.CtxUsers.Commands.CreateUser;
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
    }
}
