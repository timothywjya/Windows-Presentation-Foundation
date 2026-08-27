using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using MySql.Data.MySqlClient;
using Belajar_1.Helper;

namespace Belajar_1
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorHelper.ShowValidationError("Username dan Password tidak boleh kosong!");
                return;
            }
            string md5Password = EncryptMD5(password);

            string query = "SELECT * FROM tbmaster_user WHERE (USR_USERID = @user OR USR_USERNAME = @user) AND USR_PASSWORD = @pass";

            MySqlParameter[] parameters = {
                new MySqlParameter("@user", username),
                new MySqlParameter("@pass", md5Password)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                MainWindow mainDash = new MainWindow();
                mainDash.Show();

                this.Close();
            }
            else
            {
                ErrorHelper.ShowValidationError("Username atau Password salah!");
            }
        }
        private string EncryptMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" menghasilkan format hex huruf kecil
                }
                return sb.ToString();
            }
        }
    }
}