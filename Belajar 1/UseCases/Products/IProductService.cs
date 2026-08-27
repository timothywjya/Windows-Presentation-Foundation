using System.Collections.Generic;
using Belajar_1.Domain.Entities;
using Belajar_1.Domain.Interfaces;

namespace Belajar_1.UseCases.Products
{
    public interface IProductService
    {
        List<Product> GetAllProducts();

        List<Product> SearchProducts(ProductSearchField field, string keyword);

        bool CreateProduct(string kode, string nama, string merk, string flavour, string kemasan,
                            string size, string deskripsi, string unit, string fracText,
                            string hargaJualText, string hargaJual2Text, out string errorMessage);

        bool UpdateProduct(string kode, string nama, string merk, string flavour, string kemasan,
                            string size, string deskripsi, string unit, string fracText,
                            string hargaJualText, string hargaJual2Text, out string errorMessage);

        bool DeleteProduct(string kode, out string errorMessage);
    }
}
