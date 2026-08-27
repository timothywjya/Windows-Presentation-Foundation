using System.Collections.Generic;
using Belajar_1.Domain.Entities;
using Belajar_1.Domain.Interfaces;

namespace Belajar_1.UseCases.Products
{
    /// <summary>
    /// Orkestrasi use case Produk: validasi input & parsing angka, lalu
    /// delegasi ke IProductRepository. Isinya adalah aturan-aturan yang dulu
    /// ada di FormProduk.xaml.cs (ValidateForm + MapFormToModel), dipindah ke
    /// sini supaya View benar-benar "bodoh" — sama seperti UserService.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProducts() => _productRepository.GetAll();

        public List<Product> SearchProducts(ProductSearchField field, string keyword)
            => _productRepository.Search(field, keyword);

        public bool CreateProduct(string kode, string nama, string merk, string flavour, string kemasan,
                                   string size, string deskripsi, string unit, string fracText,
                                   string hargaJualText, string hargaJual2Text, out string errorMessage)
        {
            if (!Validate(kode, nama, fracText, hargaJualText, hargaJual2Text, out errorMessage))
                return false;

            var product = BuildEntity(kode, nama, merk, flavour, kemasan, size, deskripsi, unit,
                                       fracText, hargaJualText, hargaJual2Text);

            return _productRepository.Insert(product, out errorMessage);
        }

        public bool UpdateProduct(string kode, string nama, string merk, string flavour, string kemasan,
                                   string size, string deskripsi, string unit, string fracText,
                                   string hargaJualText, string hargaJual2Text, out string errorMessage)
        {
            if (!Validate(kode, nama, fracText, hargaJualText, hargaJual2Text, out errorMessage))
                return false;

            var product = BuildEntity(kode, nama, merk, flavour, kemasan, size, deskripsi, unit,
                                       fracText, hargaJualText, hargaJual2Text);

            return _productRepository.Update(product, out errorMessage);
        }

        public bool DeleteProduct(string kode, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(kode))
            {
                errorMessage = "Pilih produk yang ingin dihapus terlebih dahulu.";
                return false;
            }

            return _productRepository.Delete(kode, out errorMessage);
        }

        /// <summary>Sama persis dengan ValidateForm() di FormProduk.xaml.cs versi lama.</summary>
        private static bool Validate(string kode, string nama, string fracText, string hargaJualText,
                                      string hargaJual2Text, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(kode))
                errorMessage = "Kode PLU wajib diisi!";
            else if (string.IsNullOrWhiteSpace(nama))
                errorMessage = "Nama Barang wajib diisi!";
            else if (kode.Trim().Length > 10)
                errorMessage = "Kode PLU tidak boleh lebih dari 10 karakter!";
            else if (!string.IsNullOrWhiteSpace(hargaJualText) && (!decimal.TryParse(hargaJualText, out decimal hrg) || hrg < 0))
                errorMessage = "Harga Jual harus berupa angka desimal positif!";
            else if (!string.IsNullOrWhiteSpace(hargaJual2Text) && (!decimal.TryParse(hargaJual2Text, out decimal hrg2) || hrg2 < 0))
                errorMessage = "Harga Usulan harus berupa angka desimal positif!";
            else if (!string.IsNullOrWhiteSpace(fracText) && (!int.TryParse(fracText, out int frc) || frc < 0))
                errorMessage = "Frac / Unit harus berupa angka bulat positif!";
            else if (nama.Contains('\'') || nama.Contains("--"))
                errorMessage = "Nama Barang tidak boleh mengandung karakter petik (') atau double strip (--)!";

            return string.IsNullOrEmpty(errorMessage);
        }

        private static Product BuildEntity(string kode, string nama, string merk, string flavour, string kemasan,
                                            string size, string deskripsi, string unit, string fracText,
                                            string hargaJualText, string hargaJual2Text)
        {
            decimal.TryParse(hargaJualText, out decimal hargaJual);
            decimal.TryParse(hargaJual2Text, out decimal hargaJual2);
            if (!int.TryParse(fracText, out int frac)) frac = 1;

            return new Product
            {
                Kode = kode.Trim(),
                Nama = nama.Trim(),
                Merk = (merk ?? string.Empty).Trim(),
                Flavour = (flavour ?? string.Empty).Trim(),
                Kemasan = (kemasan ?? string.Empty).Trim(),
                Size = (size ?? string.Empty).Trim(),
                Deskripsi = (deskripsi ?? string.Empty).Trim(),
                Unit = string.IsNullOrWhiteSpace(unit) ? "PCS" : unit.Trim(),
                Frac = frac,
                HargaJual = hargaJual,
                HargaJual2 = hargaJual2
            };
        }
    }
}
