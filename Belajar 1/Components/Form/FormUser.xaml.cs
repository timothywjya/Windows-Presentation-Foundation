using System.Windows;
using System.Windows.Controls;
using Belajar_1.Infrastructure.Repositories;
using Belajar_1.Infrastructure.Security;
using Belajar_1.Presentation.ViewModels;
using Belajar_1.UseCases.Users;

namespace Belajar_1.Components.Form
{
    public partial class FormUser : UserControl
    {
        private readonly UserViewModel _viewModel;

        public FormUser()
        {
            InitializeComponent();

            // Composition root: di sinilah satu-satunya tempat Presentation
            // "tahu" tentang implementasi konkret Infrastructure. ViewModel
            // sendiri hanya menerima IUserService lewat constructor-nya.
            IUserService userService = new UserService(new UserRepository(), new Md5PasswordHasher());

            _viewModel = new UserViewModel(userService);
            DataContext = _viewModel;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Hapus User ini?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            if (!_viewModel.DeleteSelected(out string error))
                MessageBox.Show(error, "OMI System - Oops Something Broke", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
