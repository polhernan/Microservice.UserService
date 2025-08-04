using System.Reflection.Metadata.Ecma335;
using UserService.Domain.Entities;

namespace UserService.Application.Contexts.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : ICommand<Result<User>>
    {
        private Guid? UserId { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }

        public Guid? GetUserId()
        {
            return UserId;
        }

        public void SetUserId(Guid? UserId)
        {
            this.UserId = UserId;
        }
    }
}
