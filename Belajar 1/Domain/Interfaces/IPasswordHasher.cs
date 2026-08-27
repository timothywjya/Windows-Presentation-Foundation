namespace Belajar_1.Domain.Interfaces
{
    /// <summary>
    /// Abstraksi hashing password. UserService memanggil ini tanpa tahu
    /// algoritma apa yang dipakai di baliknya (MD5, BCrypt, dll).
    /// </summary>
    public interface IPasswordHasher
    {
        string Hash(string plainText);
    }
}
