using System.Windows;
using Belajar_1.Presentation.ViewModels;

namespace Belajar_1
{
    /// <summary>
    /// Shell aplikasi. Tidak ada lagi logika navigasi di sini — semuanya
    /// dipindah ke MainViewModel, code-behind ini hanya menyambungkan View
    /// ke ViewModel-nya (composition root untuk shell).
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
