using MySql.Data.MySqlClient;

namespace Belajar_1.Services
{
    public static class DatabaseConnection
    {
        private const string ConnectionString =
            "Server=localhost;Port=3306;Database=omi;Uid=root;Pwd=articunozapdosmoltres;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
