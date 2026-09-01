using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Belajar_1.Domain.Entities;
using Belajar_1.Domain.Interfaces;
using Belajar_1.Services;

namespace Belajar_1.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        public List<Supplier> GetAll()
        {
            var suppliers = new List<Supplier>();
            const string query = @"SELECT SUP_KodeSupplier, SUP_NamaSupplier, SUP_Singkatan,
                                           SUP_Telepon1, SUP_ContactPerson, SUP_Discontinue
                                    FROM tbmaster_supplier
                                    ORDER BY SUP_KodeSupplier
                                    LIMIT 100";

            using MySqlConnection conn = DatabaseConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);
            conn.Open();
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                suppliers.Add(MapRow(reader));

            return suppliers;
        }

        public bool Insert(Supplier supplier, out string errorMessage)
        {
            errorMessage = string.Empty;
            const string query = @"INSERT INTO tbmaster_supplier
                                   (SUP_KodeSupplier, SUP_NamaSupplier, SUP_Singkatan, SUP_Telepon1, SUP_ContactPerson, SUP_TglDaftar, SUP_Discontinue)
                                   VALUES
                                   (@kode, @nama, @singkatan, @telp, @cp, CURDATE(), 0)";
            try
            {
                using MySqlConnection conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(query, conn);
                AddParameters(cmd, supplier);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex) when (ex.Number == 1062) // duplicate key
            {
                errorMessage = $"Kode Supplier '{supplier.Kode}' sudah digunakan.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool Update(Supplier supplier, out string errorMessage)
        {
            errorMessage = string.Empty;
            const string query = @"UPDATE tbmaster_supplier SET
                                   SUP_NamaSupplier = @nama, SUP_Singkatan = @singkatan,
                                   SUP_Telepon1 = @telp, SUP_ContactPerson = @cp, SUP_TglUpdate = CURDATE()
                                   WHERE SUP_KodeSupplier = @kode";
            try
            {
                using MySqlConnection conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(query, conn);
                AddParameters(cmd, supplier);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool Delete(string kode, out string errorMessage)
        {
            errorMessage = string.Empty;
            const string query = "DELETE FROM tbmaster_supplier WHERE SUP_KodeSupplier = @kode";

            try
            {
                using MySqlConnection conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kode", kode);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private static void AddParameters(MySqlCommand cmd, Supplier s)
        {
            cmd.Parameters.AddWithValue("@kode", s.Kode);
            cmd.Parameters.AddWithValue("@nama", s.Nama);
            cmd.Parameters.AddWithValue("@singkatan", string.IsNullOrEmpty(s.Singkatan) ? DBNull.Value : (object)s.Singkatan);
            cmd.Parameters.AddWithValue("@telp", string.IsNullOrEmpty(s.Telepon) ? DBNull.Value : (object)s.Telepon);
            cmd.Parameters.AddWithValue("@cp", string.IsNullOrEmpty(s.ContactPerson) ? DBNull.Value : (object)s.ContactPerson);
        }

        private static Supplier MapRow(MySqlDataReader reader) => new()
        {
            Kode = reader["SUP_KodeSupplier"]?.ToString() ?? string.Empty,
            Nama = reader["SUP_NamaSupplier"]?.ToString() ?? string.Empty,
            Singkatan = reader["SUP_Singkatan"]?.ToString() ?? string.Empty,
            Telepon = reader["SUP_Telepon1"]?.ToString() ?? string.Empty,
            ContactPerson = reader["SUP_ContactPerson"]?.ToString() ?? string.Empty,
            IsActive = reader["SUP_Discontinue"] is DBNull || !Convert.ToBoolean(reader["SUP_Discontinue"])
        };
    }
}
