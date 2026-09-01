using System.Collections.ObjectModel;
using Belajar_1.Domain.Entities;
using Belajar_1.Helpers;
using Belajar_1.UseCases.Suppliers;

namespace Belajar_1.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel untuk FormSupplier. Sama seperti ProductViewModel &
    /// UserViewModel: tidak tahu apa-apa soal MySQL — semua itu ada di
    /// balik ISupplierService. View (FormSupplier.xaml) hanya berkomunikasi
    /// lewat binding ke properti & Command di sini.
    /// </summary>
    public class SupplierViewModel : ViewModelBase
    {
        private readonly ISupplierService _supplierService;

        public ObservableCollection<Supplier> Suppliers { get; } = new();

        private Supplier? _selectedSupplier;
        public Supplier? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (!SetField(ref _selectedSupplier, value)) return;

                if (value != null)
                {
                    Kode = value.Kode;
                    Nama = value.Nama;
                    Singkatan = value.Singkatan;
                    Telepon = value.Telepon;
                    ContactPerson = value.ContactPerson;
                    IsKodeEditable = false;
                }
            }
        }

        private string _kode = string.Empty;
        public string Kode { get => _kode; set => SetField(ref _kode, value); }

        private bool _isKodeEditable = true;
        public bool IsKodeEditable { get => _isKodeEditable; set => SetField(ref _isKodeEditable, value); }

        private string _nama = string.Empty;
        public string Nama { get => _nama; set => SetField(ref _nama, value); }

        private string _singkatan = string.Empty;
        public string Singkatan { get => _singkatan; set => SetField(ref _singkatan, value); }

        private string _telepon = string.Empty;
        public string Telepon { get => _telepon; set => SetField(ref _telepon, value); }

        private string _contactPerson = string.Empty;
        public string ContactPerson { get => _contactPerson; set => SetField(ref _contactPerson, value); }

        private string _errorMessage = string.Empty;
        public string ErrorMessage { get => _errorMessage; set => SetField(ref _errorMessage, value); }

        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand ClearCommand { get; }

        public SupplierViewModel(ISupplierService supplierService)
        {
            _supplierService = supplierService;

            AddCommand = new RelayCommand(_ => AddSupplier());
            UpdateCommand = new RelayCommand(_ => UpdateSupplier());
            ClearCommand = new RelayCommand(_ => Clear());

            LoadSuppliers();
        }

        public void LoadSuppliers()
        {
            Suppliers.Clear();
            foreach (Supplier supplier in _supplierService.GetAllSuppliers())
                Suppliers.Add(supplier);
        }

        private void AddSupplier()
        {
            if (_supplierService.CreateSupplier(Kode, Nama, Singkatan, Telepon, ContactPerson, out string error))
            {
                LoadSuppliers();
                Clear();
            }
            else
            {
                ErrorMessage = error;
            }
        }

        private void UpdateSupplier()
        {
            if (_supplierService.UpdateSupplier(Kode, Nama, Singkatan, Telepon, ContactPerson, out string error))
            {
                LoadSuppliers();
                Clear();
            }
            else
            {
                ErrorMessage = error;
            }
        }

        /// <summary>Dipanggil code-behind SETELAH user mengonfirmasi dialog "Hapus data supplier ini?".</summary>
        public bool DeleteSelected(out string error)
        {
            if (_supplierService.DeleteSupplier(Kode, out error))
            {
                LoadSuppliers();
                Clear();
                return true;
            }

            ErrorMessage = error;
            return false;
        }

        public void Clear()
        {
            SelectedSupplier = null;
            Kode = string.Empty;
            Nama = string.Empty;
            Singkatan = string.Empty;
            Telepon = string.Empty;
            ContactPerson = string.Empty;
            IsKodeEditable = true;
            ErrorMessage = string.Empty;
        }
    }
}
