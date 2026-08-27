using System.Windows;
using System.Windows.Controls;
using Belajar_1.Infrastructure.Repositories;
using Belajar_1.Infrastructure.Security;
using Belajar_1.Presentation.ViewModels;
using Belajar_1.UseCases.Users;

namespace Belajar_1.Components.Form
{
    /// <summary>
    /// View untuk fitur User. Sekarang murni "bodoh": tidak ada logika
    /// database atau validasi di sini, semua lewat binding ke UserViewModel.
    /// Satu-satunya kode yang tersisa adalah:
    ///  1. Composition root (merakit Repository + Service + ViewModel), dan
    ///  2. Dua hal yang memang tidak bisa di-binding murni di WPF:
    ///     PasswordBox.Password (dibatasi API keamanan WPF) dan
    ///     dialog konfirmasi hapus (MessageBox adalah tanggung jawab View).
    /// </summary>
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
            _viewModel.RequestClearPasswordBox += () => TxtPassword.Clear();

            DataContext = _viewModel;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = TxtPassword.Password;
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
