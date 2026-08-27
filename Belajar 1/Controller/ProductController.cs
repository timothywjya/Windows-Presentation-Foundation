using Belajar_1.Services;
using Belajar_1.Helper;
using Belajar_1.Model;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Belajar_1.Controller
{
    public class ProductController
    {
        public DataTable GetAllProducts()
        {
            string query = "SELECT PRD_PRDCD, PRD_NAMA, PRD_HRGJUAL FROM tbmaster_prodmast";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable SearchProducts(string searchBy, string keyword)
        {
            string query = $"SELECT PRD_PRDCD, PRD_NAMA, PRD_HRGJUAL FROM tbmaster_prodmast WHERE {searchBy} LIKE @keyword";
            MySqlParameter[] parameters = {
                new MySqlParameter("@keyword", $"%{keyword}%")
            };
            return DatabaseHelper.ExecuteQuery(query, parameters);
        }

        public DataRow GetProductDetail(string prdCd)
        {
            string query = "SELECT * FROM tbmaster_prodmast WHERE PRD_PRDCD = @prdcd";
            MySqlParameter[] parameters = { new MySqlParameter("@prdcd", prdCd) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            return (dt != null && dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }

        public bool InsertProduct(Product p, out string errorMsg)
        {
            errorMsg = string.Empty;
            string query = @"INSERT INTO tbmaster_prodmast 
                            (PRD_PRDCD, PRD_NAMA, PRD_MERK, PRD_FLAVOUR, PRD_KEMASAN, PRD_SIZE, PRD_DESKRIPSI, PRD_UNIT, PRD_FRAC, PRD_HRGJUAL, PRD_HRGJUAL2, PRD_TGLHRGJUAL) 
                            VALUES 
                            (@prdcd, @nama, @merk, @flavour, @kemasan, @size, @deskripsi, @unit, @frac, @hrgjual, @hrgjual2, NOW())";

            return ExecuteNonQuery(query, out errorMsg, BuildParameters(p));
        }

        public bool UpdateProduct(Product p, out string errorMsg)
        {
            errorMsg = string.Empty;
            string query = @"UPDATE tbmaster_prodmast SET 
                            PRD_NAMA = @nama, PRD_MERK = @merk, PRD_FLAVOUR = @flavour, PRD_KEMASAN = @kemasan, 
                            PRD_SIZE = @size, PRD_DESKRIPSI = @deskripsi, PRD_UNIT = @unit, PRD_FRAC = @frac, 
                            PRD_HRGJUAL = @hrgjual, PRD_HRGJUAL2 = @hrgjual2, PRD_TGLHRGJUAL = NOW()
                            WHERE PRD_PRDCD = @prdcd";

            return ExecuteNonQuery(query, out errorMsg, BuildParameters(p));
        }

        public bool DeleteProduct(string kode, out string errorMsg)
        {
            errorMsg = string.Empty;
            string query = "DELETE FROM tbmaster_prodmast WHERE PRD_PRDCD = @kode";
            return ExecuteNonQuery(query, out errorMsg, new MySqlParameter("@kode", kode));
        }

        private MySqlParameter[] BuildParameters(Product p)
        {
            return new MySqlParameter[]
            {
                new MySqlParameter("@prdcd", p.PRD_PRDCD),
                new MySqlParameter("@nama", p.PRD_NAMA),
                new MySqlParameter("@merk", string.IsNullOrEmpty(p.PRD_MERK) ? DBNull.Value : (object)p.PRD_MERK),
                new MySqlParameter("@flavour", string.IsNullOrEmpty(p.PRD_FLAVOUR) ? DBNull.Value : (object)p.PRD_FLAVOUR),
                new MySqlParameter("@kemasan", string.IsNullOrEmpty(p.PRD_KEMASAN) ? DBNull.Value : (object)p.PRD_KEMASAN),
                new MySqlParameter("@size", string.IsNullOrEmpty(p.PRD_SIZE) ? DBNull.Value : (object)p.PRD_SIZE),
                new MySqlParameter("@deskripsi", string.IsNullOrEmpty(p.PRD_DESKRIPSI) ? DBNull.Value : (object)p.PRD_DESKRIPSI),
                new MySqlParameter("@unit", p.PRD_UNIT),
                new MySqlParameter("@frac", p.PRD_FRAC),
                new MySqlParameter("@hrgjual", p.PRD_HRGJUAL),
                new MySqlParameter("@hrgjual2", p.PRD_HRGJUAL2)
            };
        }

        private bool ExecuteNonQuery(string q, out string err, params MySqlParameter[] p)
        {
            err = string.Empty;
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(q, conn))
                    {
                        cmd.Parameters.AddRange(p);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
        }
    }
}