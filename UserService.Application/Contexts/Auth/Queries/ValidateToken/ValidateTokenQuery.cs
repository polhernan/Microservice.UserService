using UserService.Application.Common.Interfaces;

namespace UserService.Application.Contexts.Auth.Queries.ValidateToken
{
    public class ValidateTokenQuery : IQuery<bool>
    {
        public string Token { get; set; }
    }
}
