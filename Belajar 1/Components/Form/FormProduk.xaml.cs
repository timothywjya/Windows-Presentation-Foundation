using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Belajar_1.Infrastructure.Repositories;
using Belajar_1.Presentation.ViewModels;
using Belajar_1.UseCases.Products;

namespace Belajar_1.Components.Form
{
    public partial class FormProduk : UserControl
    {
        private readonly ProductViewModel _viewModel;

        public FormProduk()
        {
            InitializeComponent();

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
