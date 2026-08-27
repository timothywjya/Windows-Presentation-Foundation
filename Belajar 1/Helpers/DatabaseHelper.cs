using System;
using System.Data;
using MySql.Data.MySqlClient;
using Belajar_1.Services;

namespace Belajar_1.Helper
{
    public static class DatabaseHelper
    {
        public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (parameters != null) cmd.Parameters.AddRange(parameters);

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.ShowDatabaseError($"ExecuteQuery ({query})", ex);
            }
            return dt;
        }

        public static bool ExecuteNonQuery(string query, MySqlParameter[] parameters, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open(); 

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (parameters != null) cmd.Parameters.AddRange(parameters);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                ErrorHelper.ShowDatabaseError($"ExecuteNonQuery ({query})", ex);
                return false;
            }
        }
    }
}