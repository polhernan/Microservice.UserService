using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Common.Models;
using UserService.Application.Contexts.Auth.Queries.TokenOwner;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Application.Contexts.Auth.Queries.GetTokenOwner
{
    internal class GetTokenOwnerQueryHandler : IRequestHandler<GetTokenOwnerQuery, Result<User>>
    {


        private readonly UserServiceDbContext _context;


        public GetTokenOwnerQueryHandler(UserServiceDbContext context)
        {
            _context = context;
        }


        public async Task<Result<User>> Handle(GetTokenOwnerQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
                return Result<User>.Failure("Empty Token");

            User? user = _context.Users.AsNoTracking().FirstOrDefault(x => x.RefreshTokens.Any(t => t.Token == request.Token && t.RevokedAt == null && t.ExpiresAt > DateTime.Now));

            if (user == null)
                return Result<User>.Failure("This token is not valid for any user");

            return Result<User>.Success(user);
        }
    }
}
