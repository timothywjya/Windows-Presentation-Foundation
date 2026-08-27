namespace Belajar_1.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string plainText);
    }
}
