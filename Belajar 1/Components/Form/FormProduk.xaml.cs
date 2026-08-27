using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Belajar_1.Infrastructure.Repositories;
using Belajar_1.Presentation.ViewModels;
using Belajar_1.UseCases.Products;

namespace Belajar_1.Components.Form
{
    /// <summary>
    /// View untuk fitur Produk. Sekarang murni "bodoh": tidak ada logika
    /// database atau validasi di sini, semua lewat binding ke ProductViewModel.
    /// Satu-satunya kode yang tersisa adalah:
    ///  1. Composition root (merakit Repository + Service + ViewModel), dan
    ///  2. Dialog konfirmasi hapus (MessageBox adalah tanggung jawab View,
    ///     sama seperti pola di FormUser.xaml.cs).
    /// </summary>
    public partial class FormProduk : UserControl
    {
        private readonly ProductViewModel _viewModel;

        public FormProduk()
        {
            InitializeComponent();

            // Composition root: satu-satunya tempat Presentation "tahu" tentang
            // implementasi konkret Infrastructure. ViewModel sendiri hanya
            // menerima IProductService lewat constructor-nya.
            IProductService productService = new ProductService(new ProductRepository());

            _viewModel = new ProductViewModel(productService);
            DataContext = _viewModel;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && _viewModel.SearchCommand.CanExecute(null))
                _viewModel.SearchCommand.Execute(null);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_viewModel.Kode)) return;

            if (MessageBox.Show("Hapus data produk ini?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            if (!_viewModel.DeleteSelected(out string error))
                MessageBox.Show(error, "OMI System - Oops Something Broke", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
