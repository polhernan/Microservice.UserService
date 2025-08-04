using UserService.Application.Common.Interfaces;
using UserService.Application.Common.Models;
using UserService.Domain.Common;

namespace UserService.Application.Contexts.Auth.Commands.LogIn
{
    public class LogInUserCommand : ICommand<Result<RefreshToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
