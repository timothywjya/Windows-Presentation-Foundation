using System.Windows;
using System.Windows.Controls;

namespace Belajar_1.Helpers
{
    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxAssistant),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBoundPasswordChanged));

        // Flag internal supaya event PasswordChanged tidak memicu binding
        // untuk menulis ulang PasswordBox.Password (mencegah loop tak berujung).
        private static readonly DependencyProperty IsUpdating =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxAssistant));

        public static void SetBoundPassword(DependencyObject dp, string value) => dp.SetValue(BoundPassword, value);
        public static string GetBoundPassword(DependencyObject dp) => (string)dp.GetValue(BoundPassword);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox passwordBox) return;

            passwordBox.PasswordChanged -= HandlePasswordChanged;

            if (!(bool)passwordBox.GetValue(IsUpdating))
                passwordBox.Password = (string)e.NewValue ?? string.Empty;

            passwordBox.PasswordChanged += HandlePasswordChanged;
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            passwordBox.SetValue(IsUpdating, true);
            SetBoundPassword(passwordBox, passwordBox.Password);
            passwordBox.SetValue(IsUpdating, false);
        }
    }
}
