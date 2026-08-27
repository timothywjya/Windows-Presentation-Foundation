using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Belajar_1.Components
{
    public partial class SearchControlGlobal : UserControl
    {
        public event Action<string, string> OnSearchTriggered;
        private const string PLACEHOLDER = "Ketik kata kunci...";

        public SearchControlGlobal()
        {
            InitializeComponent();
            TxtKeyword.Text = PLACEHOLDER;
            TxtKeyword.Foreground = System.Windows.Media.Brushes.Gray;
        }

        public void SetSearchOptions(List<KeyValuePair<string, string>> options)
        {
            CmbType.ItemsSource = options;
            CmbType.DisplayMemberPath = "Key";
            CmbType.SelectedValuePath = "Value";
            if (CmbType.Items.Count > 0) CmbType.SelectedIndex = 0;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e) => TriggerSearch();
        private void TxtKeyword_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) TriggerSearch(); }

        private void TriggerSearch()
        {
            string key = TxtKeyword.Text == PLACEHOLDER ? "" : TxtKeyword.Text.Trim();
            string type = CmbType.SelectedValue?.ToString() ?? "";
            OnSearchTriggered?.Invoke(type, key);
        }

        private void TxtKeyword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtKeyword.Text == PLACEHOLDER) { TxtKeyword.Text = ""; TxtKeyword.Foreground = System.Windows.Media.Brushes.Black; }
        }

        private void TxtKeyword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtKeyword.Text)) { TxtKeyword.Text = PLACEHOLDER; TxtKeyword.Foreground = System.Windows.Media.Brushes.Gray; }
        }
    }
}