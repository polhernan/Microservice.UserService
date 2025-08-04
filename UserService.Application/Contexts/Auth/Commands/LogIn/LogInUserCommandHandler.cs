using MediatR;
using Microsoft.Extensions.Options;
using UserService.Application.Common.Models;
using UserService.Domain.Common;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.Application.Contexts.Auth.Commands.LogIn
{
    public class LogInUserCommandHandler : IRequestHandler<LogInUserCommand, Result<RefreshToken>>
    {


        private readonly UserServiceDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _tokenService;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public LogInUserCommandHandler(UserServiceDbContext context, IPasswordHasher passwordHasher, IJwtTokenService tokenService, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings;
        }


        public async Task<Result<RefreshToken>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
        {
            if (!Email.EsEmailValido(request.Email))
                return Result<RefreshToken>.Failure("Email invalido");

            User? user = _context.Users.FirstOrDefault(u => u.Email.Value == request.Email);

            if (user == null)
                return Result<RefreshToken>.Failure($"User with email {request.Email} couldn't be found.");

            if(!user.VerifyPassword(request.Password, _passwordHasher))
                return Result<RefreshToken>.Failure($"Wrong password");

            string token = _tokenService.GenerateJwtToken(user);

            Result<RefreshToken> refreshTokenResult = user.AddRefreshToken(_tokenService, _jwtSettings.Value);

            if(refreshTokenResult.Succeded)
                await _context.SaveChangesAsync();

            return refreshTokenResult;
        }
    }
}
