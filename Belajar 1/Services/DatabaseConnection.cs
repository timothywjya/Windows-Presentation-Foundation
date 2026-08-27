using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Belajar_1.Services
{
    public class DatabaseConnection
    {
        private static readonly string ConnectionString =
            "Server=192.168.10.10;" +
            "Database=omi;" +
            "Uid=root;" +
            "Pwd=articunozapdosmoltres;" +
            "AllowZeroDateTime=True;" +
            "ConvertZeroDateTime=True;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return false;
                }
            }
        }
    }
}