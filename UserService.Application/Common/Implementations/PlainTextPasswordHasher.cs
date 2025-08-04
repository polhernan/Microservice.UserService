using UserService.Domain.Interfaces;

namespace UserService.Application.Common.Implementations
{
    public class PlainTextPasswordHasher : IPasswordHasher
    {
        public string Hash(string plainPassword)
        {
            return plainPassword;
        }

        public bool Verify(string hash, string plainPassword)
        {
            return hash.Equals(plainPassword);
        }
    }
}
