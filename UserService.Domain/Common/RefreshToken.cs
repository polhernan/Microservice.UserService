using Serilog;
using Serilog.Context;
using Serilog.Core;
using UserService.Application.Common.Models;

namespace UserService.Domain.Common
{
    public class RefreshToken
    {


        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public bool IsExpired => DateTime.Now < ExpiresAt;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsActive => !IsExpired && !IsRevoked;


        private RefreshToken() { }


        public static Result<RefreshToken> Create(string token, TimeSpan duration)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Result<RefreshToken>.Failure("Token can't be empty");

            var now = DateTime.Now;

            return Result<RefreshToken>.Success(new RefreshToken
            {
                Token = token,
                CreatedAt = now,
                ExpiresAt = now.Add(duration)
            });
        }

        public Result<RefreshToken> Revoke()
        {
            if (IsRevoked)
                return Result<RefreshToken>.Failure("Token is alredy revoked.");

            RevokedAt = DateTime.Now;

            return Result<RefreshToken>.Success();
        }

        public void ForceRevoke()
        {
            using (LogContext.PushProperty("Token", Token))
            using (LogContext.PushProperty("RevokedAt", RevokedAt))
            {
                Log.Warning("Performing Force Revoke");
            }

            RevokedAt = DateTime.Now;
        }

    }
}
