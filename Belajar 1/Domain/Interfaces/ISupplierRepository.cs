using System.Collections.Generic;

namespace Belajar_1.Domain.Interfaces
{
    public interface ISupplierRepository
    {
        List<Entities.Supplier> GetAll();

        bool Insert(Entities.Supplier supplier, out string errorMessage);

        bool Update(Entities.Supplier supplier, out string errorMessage);

        bool Delete(string kode, out string errorMessage);
    }
}
