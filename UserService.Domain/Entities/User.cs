using UserService.Application.Common.Models;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Entities
{
    public class User : AuditableEntity
    {


        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }
        public Email Email { get; set; }
        public string PasswordHash { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }

        
        private User() : base() { }


        public static Result<User> Create(string name, string surnames, string email, string plainPassword, IPasswordHasher passwordHasher)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<User>.Failure("Name can't be empty");

            if (string.IsNullOrWhiteSpace(surnames))
                return Result<User>.Failure("Surnames can't be empty");

            if (string.IsNullOrWhiteSpace(plainPassword))
                return Result<User>.Failure("Surnames can't be empty");

            Result<Email> emailResult = Email.Create(email);
            if (!emailResult.Succeded)
                return Result<User>.Failure(emailResult.Errors);

            string passwordHash = passwordHasher.Hash(plainPassword);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Surnames = surnames.Trim(),
                Email = emailResult.Value,
                PasswordHash = passwordHash,
                RefreshTokens = new List<RefreshToken>()
            };

            return Result<User>.Success(user);
        }

        public Result<Email> ChangeEmail(string newEmail)
        {
            if (newEmail == Email)
                return Result<Email>.Failure("Emails are the same.");

            Result<Email> emailResult = Email.Create(newEmail);
            if (!emailResult.Succeded)
                return emailResult;

            Email = emailResult.Value;

            UpdateEntity();

            return emailResult;
        }

        public Result<User> UpdatePassword(string plainPassword, IPasswordHasher hasher)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                return Result<User>.Failure("Password can't be plain.");

            if (hasher.Verify(PasswordHash, plainPassword))
                return Result<User>.Failure("Password must be different from the previous one.");

            string newPasswordHash = hasher.Hash(plainPassword);

            PasswordHash = newPasswordHash;

            UpdateEntity();

            return Result<User>.Success();
        }

        public bool VerifyPassword(string plainPassword, IPasswordHasher hasher)
        {
            return hasher.Verify(PasswordHash, plainPassword);
        }

        public Result<RefreshToken> AddRefreshToken(IJwtTokenService tokenService, JwtSettings jwtSettings, string? ipAddress)
        {
            // Testing purpouses.
            Result<RefreshToken> resultRefreshToken = RefreshToken.Create(
                tokenService.GenerateJwtToken(this), 
                new TimeSpan(0,jwtSettings.ExpirationMinutes,0),
                ipAddress
                );

            if (resultRefreshToken.Succeded)
                RefreshTokens.Add(resultRefreshToken.Value);

            return resultRefreshToken;
        }
    }
}
