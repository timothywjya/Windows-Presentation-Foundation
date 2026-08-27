using System.Windows;
using System.Windows.Media;

namespace Belajar_1.Components
{
    public partial class NotificationDialog : Window
    {
        public enum DialogType { Info, Success, Warning, Error, Confirm }
        private bool _isConfirmation = false;

        public NotificationDialog(string msg, DialogType type)
        {
            InitializeComponent();
            TxtMessage.Text = msg;
            ConfigureType(type);
        }

        private void ConfigureType(DialogType type)
        {
            switch (type)
            {
                case DialogType.Success:
                    HeaderBar.Background = (SolidColorBrush)Application.Current.Resources["ColorSuccess"];
                    TxtIcon.Text = "✅";
                    break;
                case DialogType.Error:
                    HeaderBar.Background = (SolidColorBrush)Application.Current.Resources["ColorDanger"];
                    TxtIcon.Text = "❌";
                    break;
                case DialogType.Warning:
                    HeaderBar.Background = (SolidColorBrush)Application.Current.Resources["ColorWarning"];
                    TxtIcon.Text = "⚠️";
                    break;
                case DialogType.Confirm:
                    HeaderBar.Background = (SolidColorBrush)Application.Current.Resources["ColorPrimary"];
                    TxtIcon.Text = "❓";
                    BtnCancel.Visibility = Visibility.Visible;
                    _isConfirmation = true;
                    break;
                default:
                    HeaderBar.Background = (SolidColorBrush)Application.Current.Resources["ColorPrimary"];
                    TxtIcon.Text = "ℹ️";
                    break;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) { this.DialogResult = true; this.Close(); }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; this.Close(); }

        public static bool Show(string message, DialogType type = DialogType.Info, Window owner = null)
        {
            var dialog = new NotificationDialog(message, type);
            if (owner != null) dialog.Owner = owner;
            else dialog.Owner = Application.Current.MainWindow;
            return dialog.ShowDialog() ?? false;
        }
    }
}