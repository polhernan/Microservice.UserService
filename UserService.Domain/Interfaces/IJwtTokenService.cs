using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
    }
}
