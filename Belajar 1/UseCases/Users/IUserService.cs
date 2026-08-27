using System.Collections.Generic;
using Belajar_1.Domain.Entities;

namespace Belajar_1.UseCases.Users
{
    public interface IUserService
    {
        List<User> GetAllUsers();

        bool CreateUser(string userId, string username, string password, string email, int level, out string errorMessage);

        bool UpdateUser(string userId, string username, string password, string email, int level, out string errorMessage);

        bool DeleteUser(string userId, out string errorMessage);
    }
}
