using System.Collections.Generic;
using Belajar_1.Domain.Entities;

namespace Belajar_1.Domain.Interfaces
{
    /// <summary>
    /// Kontrak akses data User. Domain & Application hanya bergantung pada
    /// interface ini, bukan pada implementasi MySQL-nya (Dependency Inversion).
    /// </summary>
    public interface IUserRepository
    {
        List<User> GetAll();

        User? Authenticate(string usernameOrUserId, string hashedPassword);

        bool Insert(User user, out string errorMessage);

        bool Update(User user, bool updatePassword, out string errorMessage);

        bool Delete(string userId, out string errorMessage);
    }
}
