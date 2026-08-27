using System;
using System.Collections.ObjectModel;
using Belajar_1.Domain.Entities;
using Belajar_1.Domain.Interfaces;
using Belajar_1.Helpers;
using Belajar_1.UseCases.Products;

namespace Belajar_1.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel untuk FormProduk. Sama seperti UserViewModel: tidak tahu
    /// apa-apa soal MySQL/ADO.NET — semua itu ada di balik IProductService.
    /// View (FormProduk.xaml) hanya berkomunikasi lewat binding ke properti
    /// & Command di sini.
    /// </summary>
    public class ProductViewModel : ViewModelBase
    {
        private readonly IProductService _productService;

        public ObservableCollection<Product> Products { get; } = new();

        // --- Pencarian ---
        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set => SetField(ref _searchKeyword, value);
        }

        private ProductSearchField _searchField = ProductSearchField.Kode;
        public ProductSearchField SearchField
        {
            get => _searchField;
            set => SetField(ref _searchField, value);
        }

        // --- Form detail (mengikuti field yang dulu ada di FormProduk.xaml.cs) ---
        private Product? _selectedProduct;
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (!SetField(ref _selectedProduct, value)) return;

                if (value != null)
                {
                    Kode = value.Kode;
                    Nama = value.Nama;
                    Merk = value.Merk;
                    Flavour = value.Flavour;
                    Kemasan = value.Kemasan;
                    Size = value.Size;
                    Deskripsi = value.Deskripsi;
                    Unit = value.Unit;
                    FracText = value.Frac.ToString();
                    HargaJualText = value.HargaJual.ToString();
                    HargaJual2Text = value.HargaJual2.ToString();
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

        private string _merk = string.Empty;
        public string Merk { get => _merk; set => SetField(ref _merk, value); }

        private string _flavour = string.Empty;
        public string Flavour { get => _flavour; set => SetField(ref _flavour, value); }

        private string _kemasan = string.Empty;
        public string Kemasan { get => _kemasan; set => SetField(ref _kemasan, value); }

        private string _size = string.Empty;
        public string Size { get => _size; set => SetField(ref _size, value); }

        private string _deskripsi = string.Empty;
        public string Deskripsi { get => _deskripsi; set => SetField(ref _deskripsi, value); }

        private string _unit = "PCS";
        public string Unit { get => _unit; set => SetField(ref _unit, value); }

        private string _fracText = "1";
        public string FracText { get => _fracText; set => SetField(ref _fracText, value); }

        private string _hargaJualText = string.Empty;
        public string HargaJualText { get => _hargaJualText; set => SetField(ref _hargaJualText, value); }

        private string _hargaJual2Text = string.Empty;
        public string HargaJual2Text { get => _hargaJual2Text; set => SetField(ref _hargaJual2Text, value); }

        private string _errorMessage = string.Empty;
        public string ErrorMessage { get => _errorMessage; set => SetField(ref _errorMessage, value); }

        public RelayCommand SearchCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand ClearCommand { get; }

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;

            SearchCommand = new RelayCommand(_ => Search());
            AddCommand = new RelayCommand(_ => AddProduct());
            UpdateCommand = new RelayCommand(_ => UpdateProduct());
            ClearCommand = new RelayCommand(_ => Clear());

            LoadProducts();
        }

        public void LoadProducts()
        {
            Products.Clear();
            foreach (Product product in _productService.GetAllProducts())
                Products.Add(product);
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                LoadProducts();
                return;
            }

            Products.Clear();
            foreach (Product product in _productService.SearchProducts(SearchField, SearchKeyword.Trim()))
                Products.Add(product);
        }

        private void AddProduct()
        {
            if (_productService.CreateProduct(Kode, Nama, Merk, Flavour, Kemasan, Size, Deskripsi, Unit,
                    FracText, HargaJualText, HargaJual2Text, out string error))
            {
                LoadProducts();
                Clear();
            }
            else
            {
                ErrorMessage = error;
            }
        }

        private void UpdateProduct()
        {
            if (_productService.UpdateProduct(Kode, Nama, Merk, Flavour, Kemasan, Size, Deskripsi, Unit,
                    FracText, HargaJualText, HargaJual2Text, out string error))
            {
                LoadProducts();
                Clear();
            }
            else
            {
                ErrorMessage = error;
            }
        }

        /// <summary>
        /// Dipanggil oleh code-behind SETELAH user mengonfirmasi dialog
        /// "Hapus data produk ini?" — sama seperti UserViewModel.DeleteSelected().
        /// </summary>
        public bool DeleteSelected(out string error)
        {
            if (_productService.DeleteProduct(Kode, out error))
            {
                LoadProducts();
                Clear();
                return true;
            }

            ErrorMessage = error;
            return false;
        }

        public void Clear()
        {
            SelectedProduct = null;
            Kode = string.Empty;
            Nama = string.Empty;
            Merk = string.Empty;
            Flavour = string.Empty;
            Kemasan = string.Empty;
            Size = string.Empty;
            Deskripsi = string.Empty;
            Unit = "PCS";
            FracText = "1";
            HargaJualText = string.Empty;
            HargaJual2Text = string.Empty;
            IsKodeEditable = true;
            ErrorMessage = string.Empty;
        }
    }
}
