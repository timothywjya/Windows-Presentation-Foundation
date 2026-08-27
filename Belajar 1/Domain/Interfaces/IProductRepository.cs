using System.Collections.Generic;

namespace Belajar_1.Domain.Interfaces
{
    public enum ProductSearchField
    {
        Kode,
        Deskripsi
    }

    public interface IProductRepository
    {
        List<Entities.Product> GetAll();

        List<Entities.Product> Search(ProductSearchField field, string keyword);

        Entities.Product? GetByCode(string kode);

        bool Insert(Entities.Product product, out string errorMessage);

        bool Update(Entities.Product product, out string errorMessage);

        bool Delete(string kode, out string errorMessage);
    }
}
