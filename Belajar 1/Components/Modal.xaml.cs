using System.Windows;
using System.Windows.Controls;

namespace Belajar_1.Components
{    public partial class Modal : UserControl
    {
        public Modal()
        {
            InitializeComponent();
        }

        public void Open(string title, UserControl content)
        {
            TxtTitle.Text = title;
            ModalBody.Content = content;
            this.Visibility = Visibility.Visible;
        }

        public void Close()
        {
            this.Visibility = Visibility.Collapsed;
            ModalBody.Content = null;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}