using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Belajar_1.Domain.Entities;
using Belajar_1.Domain.Interfaces;
using Belajar_1.Services;

namespace Belajar_1.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public List<User> GetAll()
        {
            var users = new List<User>();

            const string query = @"SELECT USR_USERID, USR_USERNAME, USR_EMAIL, USR_USERLEVEL
                                    FROM tbmaster_user
                                    ORDER BY USR_USERNAME
                                    LIMIT 100";

            using MySqlConnection conn = DatabaseConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);
            conn.Open();
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    UserId = reader["USR_USERID"].ToString() ?? string.Empty,
                    Username = reader["USR_USERNAME"].ToString() ?? string.Empty,
                    Email = reader["USR_EMAIL"].ToString() ?? string.Empty,
                    UserLevel = Convert.ToInt32(reader["USR_USERLEVEL"])
                });
            }

            return users;
        }

        public User? Authenticate(string usernameOrUserId, string hashedPassword)
        {
            const string query = @"SELECT USR_USERID, USR_USERNAME, USR_EMAIL, USR_USERLEVEL
                                    FROM tbmaster_user
                                    WHERE (USR_USERID = @user OR USR_USERNAME = @user)
                                      AND USR_PASSWORD = @pass";

            using MySqlConnection conn = DatabaseConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@user", usernameOrUserId);
            cmd.Parameters.AddWithValue("@pass", hashedPassword);

            conn.Open();
            using MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new User
            {
                UserId = reader["USR_USERID"].ToString() ?? string.Empty,
                Username = reader["USR_USERNAME"].ToString() ?? string.Empty,
                Email = reader["USR_EMAIL"].ToString() ?? string.Empty,
                UserLevel = Convert.ToInt32(reader["USR_USERLEVEL"])
            };
        }

        public bool Insert(User user, out string errorMessage)
        {
            errorMessage = string.Empty;
            const string query = @"INSERT INTO tbmaster_user
                                   (USR_USERID, USR_PASSWORD, USR_USERNAME, USR_EMAIL, USR_USERLEVEL, USR_CREATE_DT)
                                   VALUES
                                   (@userId, @password, @username, @email, @level, NOW())";
            try
            {
                using MySqlConnection conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", user.UserId);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@level", user.UserLevel);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex) when (ex.Number == 1062) // duplicate key
            {
                errorMessage = $"User ID '{user.UserId}' sudah digunakan.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool Update(User user, bool updatePassword, out string errorMessage)
        {
            errorMessage = string.Empty;

            string query = updatePassword
                ? @"UPDATE tbmaster_user SET
                        USR_PASSWORD = @password, USR_USERNAME = @username,
                        USR_EMAIL = @email, USR_USERLEVEL = @level, USR_MODIFY_DT = NOW()
                    WHERE USR_USERID = @userId"
                : @"UPDATE tbmaster_user SET
                        USR_USERNAME = @username, USR_EMAIL = @email,
                        USR_USERLEVEL = @level, USR_MODIFY_DT = NOW()
                    WHERE USR_USERID = @userId";

            try
            {
                using MySqlConnection conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", user.UserId);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@level", user.UserLevel);
                if (updatePassword)
                    cmd.Parameters.AddWithValue("@password", user.Password);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool Delete(string userId, out string errorMessage)
        {
            errorMessage = string.Empty;
            const string query = "DELETE FROM tbmaster_user WHERE USR_USERID = @userId";

            try
            {
                using MySqlConnection conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
