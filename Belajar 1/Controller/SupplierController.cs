using System;
using System.Data;
using MySql.Data.MySqlClient;
using Belajar_1.Services;

namespace Belajar_1.Controller
{
    public class SupplierController
    {
        public DataTable GetAllSuppliers()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
                                SUP_KodeSupplier AS 'Kode Supplier', 
                                SUP_NamaSupplier AS 'Nama Supplier', 
                                SUP_Singkatan AS 'Singkatan', 
                                SUP_Telepon1 AS 'Telepon', 
                                SUP_ContactPerson AS 'Contact Person',
                                IF(SUP_Discontinue = 1, 'Tidak Aktif', 'Aktif') AS 'Status'
                             FROM tbmaster_supplier 
                             LIMIT 100";

            return ExecuteQuery(query);
        }

        public bool InsertSupplier(string kode, string nama, string singkatan, string telp, string cp, out string errorMsg)
        {
            errorMsg = string.Empty;
            string query = @"INSERT INTO tbmaster_supplier 
                             (SUP_KodeSupplier, SUP_NamaSupplier, SUP_Singkatan, SUP_Telepon1, SUP_ContactPerson, SUP_TglDaftar, SUP_Discontinue) 
                             VALUES 
                             (@kode, @nama, @singkatan, @telp, @cp, CURDATE(), 0)";

            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kode", kode);
                        cmd.Parameters.AddWithValue("@nama", nama);
                        cmd.Parameters.AddWithValue("@singkatan", singkatan);
                        cmd.Parameters.AddWithValue("@telp", telp);
                        cmd.Parameters.AddWithValue("@cp", cp);

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
        public bool UpdateSupplier(string kode, string nama, string singkatan, string telp, string cp, out string errorMsg)
        {
            errorMsg = string.Empty;
            string query = @"UPDATE tbmaster_supplier SET 
                        SUP_NamaSupplier = @nama, SUP_Singkatan = @singkatan, 
                        SUP_Telepon1 = @telp, SUP_ContactPerson = @cp, SUP_TglUpdate = CURDATE()
                     WHERE SUP_KodeSupplier = @kode";
            return ExecuteNonQuery(query, out errorMsg,
                new MySqlParameter("@kode", kode),
                new MySqlParameter("@nama", nama),
                new MySqlParameter("@singkatan", singkatan),
                new MySqlParameter("@telp", telp),
                new MySqlParameter("@cp", cp));
        }
        public bool DeleteSupplier(string kode, out string errorMsg)
        {
            errorMsg = string.Empty;
            string query = "DELETE FROM tbmaster_supplier WHERE SUP_KodeSupplier = @kode";
            return ExecuteNonQuery(query, out errorMsg, new MySqlParameter("@kode", kode));
        }
        private bool ExecuteNonQuery(string query, out string errorMsg, params MySqlParameter[] parameters)
        {
            errorMsg = string.Empty;
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
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
                System.Diagnostics.Debug.WriteLine("Error Supplier Query: " + ex.Message);
            }
            return dt;
        }
    }
}