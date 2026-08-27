using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Belajar_1.Domain.Entities;
using Belajar_1.Domain.Interfaces;
using Belajar_1.Services;

namespace Belajar_1.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private const string SelectColumns = @"PRD_PRDCD, PRD_NAMA, PRD_MERK, PRD_FLAVOUR, PRD_KEMASAN,
                                                 PRD_SIZE, PRD_DESKRIPSI, PRD_UNIT, PRD_FRAC, PRD_HRGJUAL, PRD_HRGJUAL2";

        public List<Product> GetAll()
        {
            var products = new List<Product>();
            string query = $"SELECT {SelectColumns} FROM tbmaster_prodmast ORDER BY PRD_PRDCD";

            using MySqlConnection conn = DatabaseConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);
            conn.Open();
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                products.Add(MapRow(reader));

            return products;
        }

        public List<Product> Search(ProductSearchField field, string keyword)
        {
            var products = new List<Product>();
            string column = field == ProductSearchField.Kode ? "PRD_PRDCD" : "PRD_DESKRIPSI";
            string query = $"SELECT {SelectColumns} FROM tbmaster_prodmast WHERE {column} LIKE @keyword ORDER BY PRD_PRDCD";

            using MySqlConnection conn = DatabaseConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

            conn.Open();
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                products.Add(MapRow(reader));

            return products;
        }

        public Product? GetByCode(string kode)
        {
            string query = $"SELECT {SelectColumns} FROM tbmaster_prodmast WHERE PRD_PRDCD = @kode";

            using MySqlConnection conn = DatabaseConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@kode", kode);

            conn.Open();
            using MySqlDataReader reader = cmd.ExecuteReader();
            return reader.Read() ? MapRow(reader) : null;
        }

        public bool Insert(Product product, out string errorMessage)
        {
            errorMessage = string.Empty;
            const string query = @"INSERT INTO tbmaster_prodmast
                                   (PRD_PRDCD, PRD_NAMA, PRD_MERK, PRD_FLAVOUR, PRD_KEMASAN, PRD_SIZE, PRD_DESKRIPSI, PRD_UNIT, PRD_FRAC, PRD_HRGJUAL, PRD_HRGJUAL2, PRD_TGLHRGJUAL)
                                   VALUES
                                   (@kode, @nama, @merk, @flavour, @kemasan, @size, @deskripsi, @unit, @frac, @hrgjual, @hrgjual2, NOW())";
            try
            {
                using MySqlConnection conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(query, conn);
                AddParameters(cmd, product);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex) when (ex.Number == 1062) // duplicate key
            {
                errorMessage = $"Kode PLU '{product.Kode}' sudah digunakan.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool Update(Product product, out string errorMessage)
        {
            errorMessage = string.Empty;
            const string query = @"UPDATE tbmaster_prodmast SET
                                   PRD_NAMA = @nama, PRD_MERK = @merk, PRD_FLAVOUR = @flavour, PRD_KEMASAN = @kemasan,
                                   PRD_SIZE = @size, PRD_DESKRIPSI = @deskripsi, PRD_UNIT = @unit, PRD_FRAC = @frac,
                                   PRD_HRGJUAL = @hrgjual, PRD_HRGJUAL2 = @hrgjual2, PRD_TGLHRGJUAL = NOW()
                                   WHERE PRD_PRDCD = @kode";
            try
            {
                using MySqlConnection conn = DatabaseConnection.GetConnection();
                using var cmd = new MySqlCommand(query, conn);
                AddParameters(cmd, product);

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
            const string query = "DELETE FROM tbmaster_prodmast WHERE PRD_PRDCD = @kode";

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

        private static void AddParameters(MySqlCommand cmd, Product p)
        {
            cmd.Parameters.AddWithValue("@kode", p.Kode);
            cmd.Parameters.AddWithValue("@nama", p.Nama);
            cmd.Parameters.AddWithValue("@merk", string.IsNullOrEmpty(p.Merk) ? DBNull.Value : (object)p.Merk);
            cmd.Parameters.AddWithValue("@flavour", string.IsNullOrEmpty(p.Flavour) ? DBNull.Value : (object)p.Flavour);
            cmd.Parameters.AddWithValue("@kemasan", string.IsNullOrEmpty(p.Kemasan) ? DBNull.Value : (object)p.Kemasan);
            cmd.Parameters.AddWithValue("@size", string.IsNullOrEmpty(p.Size) ? DBNull.Value : (object)p.Size);
            cmd.Parameters.AddWithValue("@deskripsi", string.IsNullOrEmpty(p.Deskripsi) ? DBNull.Value : (object)p.Deskripsi);
            cmd.Parameters.AddWithValue("@unit", p.Unit);
            cmd.Parameters.AddWithValue("@frac", p.Frac);
            cmd.Parameters.AddWithValue("@hrgjual", p.HargaJual);
            cmd.Parameters.AddWithValue("@hrgjual2", p.HargaJual2);
        }

        private static Product MapRow(MySqlDataReader reader) => new()
        {
            Kode = reader["PRD_PRDCD"]?.ToString() ?? string.Empty,
            Nama = reader["PRD_NAMA"]?.ToString() ?? string.Empty,
            Merk = reader["PRD_MERK"]?.ToString() ?? string.Empty,
            Flavour = reader["PRD_FLAVOUR"]?.ToString() ?? string.Empty,
            Kemasan = reader["PRD_KEMASAN"]?.ToString() ?? string.Empty,
            Size = reader["PRD_SIZE"]?.ToString() ?? string.Empty,
            Deskripsi = reader["PRD_DESKRIPSI"]?.ToString() ?? string.Empty,
            Unit = reader["PRD_UNIT"]?.ToString() ?? "PCS",
            Frac = reader["PRD_FRAC"] is DBNull ? 1 : Convert.ToInt32(reader["PRD_FRAC"]),
            HargaJual = reader["PRD_HRGJUAL"] is DBNull ? 0 : Convert.ToDecimal(reader["PRD_HRGJUAL"]),
            HargaJual2 = reader["PRD_HRGJUAL2"] is DBNull ? 0 : Convert.ToDecimal(reader["PRD_HRGJUAL2"])
        };
    }
}
