using Belajar_1.Components;
using System.Windows;
using System.Windows.Controls;
using Belajar_1.Components.Form;

namespace Belajar_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadModule(new FormProduk(), "Master Product / Manajemen Produk");
        }

        private void LoadModule(UserControl contentPage, string pageTitle)
        {
            TxtNavbarTitle.Text = pageTitle;
            MainContentFrame.Content = contentPage;
        }

        private void MenuProduk_Click(object sender, RoutedEventArgs e)
        {
            LoadModule(new FormProduk(), "Product Master)");
        }

        private void MenuSupplier_Click(object sender, RoutedEventArgs e)
        {
            LoadModule(new FormSupplier(), "Master Data Supplier");
        }

        private void MenuUser_Click(object sender, RoutedEventArgs e)
        {
            LoadModule(new FormUser(), "Management User");
        }
    }
}