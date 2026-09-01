using Belajar_1.Components.Form;
using Belajar_1.Helpers;

namespace Belajar_1.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel untuk shell aplikasi (MainWindow). Mengganti 3 event handler
    /// terpisah (MenuProduk_Click, MenuSupplier_Click, MenuUser_Click) dengan
    /// SATU Command yang di-bind dari XAML lewat CommandParameter — navigasi
    /// pun jadi lewat Binding, bukan event code-behind.
    ///
    /// Catatan: berbeda dengan ProductViewModel/SupplierViewModel/UserViewModel
    /// yang murni tidak tahu apa-apa soal WPF, MainViewModel ini adalah
    /// "ViewModel milik shell" — wajar kalau ia tahu Views mana yang harus
    /// ditampilkan, karena tugasnya memang navigasi antar halaman, bukan
    /// logika bisnis (setiap FormXxx tetap merakit ViewModel/Service/
    /// Repository-nya sendiri lewat composition root masing-masing).
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            private set => SetField(ref _currentView, value);
        }

        private string _pageTitle = "Selamat Datang";
        public string PageTitle
        {
            get => _pageTitle;
            private set => SetField(ref _pageTitle, value);
        }

        public RelayCommand NavigateCommand { get; }

        public MainViewModel()
        {
            NavigateCommand = new RelayCommand(target => Navigate(target as string));

            // Halaman default saat dashboard pertama kali dibuka.
            Navigate("produk");
        }

        private void Navigate(string? target)
        {
            switch (target)
            {
                case "produk":
                    CurrentView = new FormProduk();
                    PageTitle = "Master Data / Manajemen Produk";
                    break;
                case "supplier":
                    CurrentView = new FormSupplier();
                    PageTitle = "Master Data Supplier";
                    break;
                case "user":
                    CurrentView = new FormUser();
                    PageTitle = "Management User";
                    break;
            }
        }
    }
}
