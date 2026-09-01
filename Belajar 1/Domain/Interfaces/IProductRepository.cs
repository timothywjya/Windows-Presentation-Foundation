using System.Collections.Generic;

namespace Belajar_1.Domain.Interfaces
{
    /// <summary>Field yang boleh dicari. Enum ini yang membuat Domain tidak perlu tahu nama kolom SQL asli (PRD_PRDCD/PRD_DESKRIPSI) — pemetaan ke kolom dilakukan di Infrastructure.</summary>
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
