namespace UserService.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string plainPassword);
        bool Verify(string hash, string plainPassword);
    }
}
