using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Belajar_1.Helpers
{
    /// <summary>
    /// Dipakai untuk menampilkan teks watermark ("Ketik kata kunci...") di
    /// kotak pencarian FormProduk secara MURNI lewat binding — pengganti
    /// trik GotFocus/LostFocus yang dulu ada di code-behind FormProduk.xaml.cs.
    /// Visible saat string kosong, Collapsed saat sudah diisi.
    /// </summary>
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => string.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
