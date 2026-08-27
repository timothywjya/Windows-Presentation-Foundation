using System;
using System.Windows;

namespace Belajar_1.Helper
{
    public static class ErrorHelper
    {
        public static void ShowDatabaseError(string operation, Exception ex)
        {
            string detailMessage = $"Terjadi kesalahan saat operasi: {operation}\n\n" +
                                   $"Pesan Eror: {ex.Message}\n\n" +
                                   $"Target Source: {ex.Source}";

            MessageBox.Show(detailMessage, "OMI System - Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowValidationError(string message)
        {
            MessageBox.Show(message, "OMI System - Oops Something Broke", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}