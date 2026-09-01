using System.Collections.Generic;
using Belajar_1.Domain.Entities;

namespace Belajar_1.UseCases.Suppliers
{
    /// <summary>
    /// Use case untuk fitur Supplier. Di-inject ke SupplierViewModel —
    /// ViewModel tidak pernah tahu soal repository atau koneksi database.
    /// </summary>
    public interface ISupplierService
    {
        List<Supplier> GetAllSuppliers();

        bool CreateSupplier(string kode, string nama, string singkatan, string telepon, string contactPerson, out string errorMessage);

        bool UpdateSupplier(string kode, string nama, string singkatan, string telepon, string contactPerson, out string errorMessage);

        bool DeleteSupplier(string kode, out string errorMessage);
    }
}
