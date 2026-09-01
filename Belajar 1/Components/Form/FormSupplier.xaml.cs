using System.Windows;
using System.Windows.Controls;
using Belajar_1.Infrastructure.Repositories;
using Belajar_1.Presentation.ViewModels;
using Belajar_1.UseCases.Suppliers;

namespace Belajar_1.Components.Form
{
    public partial class FormSupplier : UserControl
    {
        private readonly SupplierViewModel _viewModel;

        public FormSupplier()
        {
            InitializeComponent();

            // Composition root: satu-satunya tempat Presentation "tahu" tentang
            // implementasi konkret Infrastructure. ViewModel sendiri hanya
            // menerima ISupplierService lewat constructor-nya.
            ISupplierService supplierService = new SupplierService(new SupplierRepository());

            _viewModel = new SupplierViewModel(supplierService);
            DataContext = _viewModel;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_viewModel.Kode)) return;

            if (MessageBox.Show("Hapus data supplier ini?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            if (!_viewModel.DeleteSelected(out string error))
                MessageBox.Show(error, "OMI System - Oops Something Broke", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
