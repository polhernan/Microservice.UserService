using UserService.Application.Common.Interfaces;
using UserService.Application.Common.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Contexts.CtxUsers.Commands.CreateUser
{
    public class CreateUserCommand : ICommand<Result<User>>
    {


        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
