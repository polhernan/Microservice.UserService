using UserService.Application.Common.Interfaces;
using UserService.Application.Common.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Contexts.Auth.Queries.TokenOwner
{
    public class GetTokenOwnerQuery : IQuery<Result<User>>
    {
        public string Token {  get; set; }
    }
}
