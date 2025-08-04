using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Data;

namespace UserService.Application.Contexts.Auth.Queries.ValidateToken
{
    internal class ValidateTokenQueryHandler : IRequestHandler<ValidateTokenQuery, bool>
    {


        private readonly UserServiceDbContext _context;


        public ValidateTokenQueryHandler(UserServiceDbContext context)
        {
            _context = context;
        }


        public async Task<bool> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
        {
            bool isValid = _context.Users.AsNoTracking().Where(x => x.RefreshTokens.Any(t => t.Token == request.Token && t.RevokedAt == null && t.ExpiresAt > DateTime.Now)).Any();

            return isValid;
        }
    }
}
