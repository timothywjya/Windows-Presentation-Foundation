using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Belajar_1.Components
{
    public partial class SearchControl : UserControl
    {
        // Event delegasi agar bisa dibaca oleh Form Utama
        public event EventHandler<SearchEventArgs> OnSearchExecuted;

        private const string PLACEHOLDER = "Ketik kata kunci...";

        public SearchControl()
        {
            InitializeComponent();
            SetPlaceholder();
        }

        private void SetPlaceholder()
        {
            TxtSearch.Text = PLACEHOLDER;
            TxtSearch.Foreground = System.Windows.Media.Brushes.Gray;
        }

        private void TxtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtSearch.Text == PLACEHOLDER)
            {
                TxtSearch.Text = "";
                TxtSearch.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TxtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                SetPlaceholder();
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSearch();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteSearch();
            }
        }

        private void ExecuteSearch()
        {
            string keyword = TxtSearch.Text == PLACEHOLDER ? "" : TxtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                Helper.ErrorHelper.ShowValidationError("Silakan masukkan kata kunci pencarian terlebih dahulu!");
                return;
            }

            var selectedItem = CmbSearchType.SelectedItem as ComboBoxItem;
            string searchBy = selectedItem?.Tag.ToString() ?? "PRD_PRDCD";

            OnSearchExecuted?.Invoke(this, new SearchEventArgs(searchBy, keyword));
        }
    }

    public class SearchEventArgs : EventArgs
    {
        public string SearchBy { get; }
        public string Keyword { get; }

        public SearchEventArgs(string searchBy, string keyword)
        {
            SearchBy = searchBy;
            Keyword = keyword;
        }
    }
}