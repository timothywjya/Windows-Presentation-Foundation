using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using Belajar_1.Services;

namespace Belajar_1.Controller
{
    public class UserController
    {
        private string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
                                USR_USERID AS 'User ID', 
                                USR_USERNAME AS 'Nama User', 
                                USR_EMAIL AS 'Email', 
                                USR_USERLEVEL AS 'Level' 
                             FROM tbmaster_user 
                             LIMIT 100";

            return ExecuteQuery(query);
        }

        public bool InsertUser(string userId, string password, string username, string email, int level, out string errorMsg)
        {
            errorMsg = string.Empty;
            string encryptedPassword = GetMD5Hash(password); 

            string query = @"INSERT INTO tbmaster_user 
                             (USR_USERID, USR_PASSWORD, USR_USERNAME, USR_EMAIL, USR_USERLEVEL, USR_CREATE_DT) 
                             VALUES 
                             (@userId, @password, @username, @email, @level, NOW())";

            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@password", encryptedPassword);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@level", level);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        public bool UpdateUser(string userId, string password, string username, string email, int level, out string errorMsg)
        {
            errorMsg = string.Empty;
            string query;

            if (!string.IsNullOrEmpty(password))
            {
                query = @"UPDATE tbmaster_user SET 
                    USR_PASSWORD = @password, USR_USERNAME = @username, 
                    USR_EMAIL = @email, USR_USERLEVEL = @level, USR_MODIFY_DT = NOW()
                  WHERE USR_USERID = @userId";
            }
            else
            {
                query = @"UPDATE tbmaster_user SET 
                    USR_USERNAME = @username, USR_EMAIL = @email, 
                    USR_USERLEVEL = @level, USR_MODIFY_DT = NOW()
                  WHERE USR_USERID = @userId";
            }

            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@level", level);
                        if (!string.IsNullOrEmpty(password))
                            cmd.Parameters.AddWithValue("@password", GetMD5Hash(password));

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
                }
            }
        }

        public bool DeleteUser(string userId, out string errorMsg)
        {
            errorMsg = string.Empty;
            string query = "DELETE FROM tbmaster_user WHERE USR_USERID = @userId";

            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        private DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error User Query: " + ex.Message);
            }
            return dt;
        }
    }
}