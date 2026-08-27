using System.Collections.Generic;
using Belajar_1.Domain.Entities;
using Belajar_1.Domain.Interfaces;

namespace Belajar_1.UseCases.Users
{
    /// <summary>
    /// Orkestrasi use case User: validasi input, hashing password, lalu
    /// delegasi ke IUserRepository. Semua aturan bisnis "Fitur User" hidup
    /// di sini, terpisah dari UI (ViewModel/View) maupun akses data (Infrastructure).
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public List<User> GetAllUsers() => _userRepository.GetAll();

        public bool CreateUser(string userId, string username, string password, string email, int level, out string errorMessage)
        {
            if (!Validate(userId, username, password, email, level, isNewUser: true, out errorMessage))
                return false;

            var user = new User
            {
                UserId = userId.Trim(),
                Username = username.Trim(),
                Password = _passwordHasher.Hash(password),
                Email = email.Trim(),
                UserLevel = level
            };

            return _userRepository.Insert(user, out errorMessage);
        }

        public bool UpdateUser(string userId, string username, string password, string email, int level, out string errorMessage)
        {
            if (!Validate(userId, username, password, email, level, isNewUser: false, out errorMessage))
                return false;

            bool changePassword = !string.IsNullOrWhiteSpace(password);

            var user = new User
            {
                UserId = userId.Trim(),
                Username = username.Trim(),
                Password = changePassword ? _passwordHasher.Hash(password) : string.Empty,
                Email = email.Trim(),
                UserLevel = level
            };

            return _userRepository.Update(user, changePassword, out errorMessage);
        }

        public bool DeleteUser(string userId, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(userId))
            {
                errorMessage = "Pilih user yang ingin dihapus terlebih dahulu.";
                return false;
            }

            return _userRepository.Delete(userId, out errorMessage);
        }

        private static bool Validate(string userId, string username, string password, string email, int level, bool isNewUser, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(userId))
                errorMessage = "User ID wajib diisi.";
            else if (string.IsNullOrWhiteSpace(username))
                errorMessage = "Username wajib diisi.";
            else if (isNewUser && string.IsNullOrWhiteSpace(password))
                errorMessage = "Password wajib diisi untuk user baru.";
            else if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                errorMessage = "Format email tidak valid.";
            else if (level <= 0)
                errorMessage = "User level tidak valid.";

            return string.IsNullOrEmpty(errorMessage);
        }
    }
}
