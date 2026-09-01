using System.Collections.Generic;
using Belajar_1.Domain.Entities;
using Belajar_1.Domain.Interfaces;

namespace Belajar_1.UseCases.Suppliers
{
    /// <summary>
    /// Orkestrasi use case Supplier: validasi input lalu delegasi ke
    /// ISupplierRepository. Sama seperti ProductService & UserService.
    /// </summary>
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public List<Supplier> GetAllSuppliers() => _supplierRepository.GetAll();

        public bool CreateSupplier(string kode, string nama, string singkatan, string telepon, string contactPerson, out string errorMessage)
        {
            if (!Validate(kode, nama, out errorMessage)) return false;

            var supplier = BuildEntity(kode, nama, singkatan, telepon, contactPerson);
            return _supplierRepository.Insert(supplier, out errorMessage);
        }

        public bool UpdateSupplier(string kode, string nama, string singkatan, string telepon, string contactPerson, out string errorMessage)
        {
            if (!Validate(kode, nama, out errorMessage)) return false;

            var supplier = BuildEntity(kode, nama, singkatan, telepon, contactPerson);
            return _supplierRepository.Update(supplier, out errorMessage);
        }

        public bool DeleteSupplier(string kode, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(kode))
            {
                errorMessage = "Pilih supplier yang ingin dihapus terlebih dahulu.";
                return false;
            }

            return _supplierRepository.Delete(kode, out errorMessage);
        }

        private static bool Validate(string kode, string nama, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(kode))
                errorMessage = "Kode Supplier wajib diisi!";
            else if (string.IsNullOrWhiteSpace(nama))
                errorMessage = "Nama Supplier wajib diisi!";
            else if (kode.Trim().Length > 10)
                errorMessage = "Kode Supplier tidak boleh lebih dari 10 karakter!";

            return string.IsNullOrEmpty(errorMessage);
        }

        private static Supplier BuildEntity(string kode, string nama, string singkatan, string telepon, string contactPerson) => new()
        {
            Kode = kode.Trim(),
            Nama = nama.Trim(),
            Singkatan = (singkatan ?? string.Empty).Trim(),
            Telepon = (telepon ?? string.Empty).Trim(),
            ContactPerson = (contactPerson ?? string.Empty).Trim()
        };
    }
}
