using Belajar_1.Controller;
using Belajar_1.Helper;
using Belajar_1.Model;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Belajar_1.Components.Form
{
    public partial class FormProduk : UserControl
    {
        private ProductController _controller = new ProductController();
        private const string PLACEHOLDER = "Ketik kata kunci...";

        public FormProduk()
        {
            InitializeComponent();
            RefreshData();
            InitializeSearchBox();
            GridProduct.GridData.SelectionChanged += GridData_SelectionChanged;
        }

        public void RefreshData()
        {
            GridProduct.SetDataSource(_controller.GetAllProducts());
        }

        private void InitializeSearchBox()
        {
            TxtSearch.Text = PLACEHOLDER;
            TxtSearch.Foreground = System.Windows.Media.Brushes.Gray;
        }

        // --- VALIDATION ENGINE ---
        private bool ValidateForm(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(TxtCode.Text))
            {
                errorMessage = "Kode PLU wajib diisi!";
                TxtCode.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                errorMessage = "Nama Barang wajib diisi!";
                TxtName.Focus();
                return false;
            }
            if (TxtCode.Text.Trim().Length > 10)
            {
                errorMessage = "Kode PLU tidak boleh lebih dari 10 karakter!";
                TxtCode.Focus();
                return false;
            }
            if (!string.IsNullOrWhiteSpace(TxtPrice.Text) && (!decimal.TryParse(TxtPrice.Text, out decimal hrg) || hrg < 0))
            {
                errorMessage = "Harga Jual harus berupa angka desimal positif!";
                TxtPrice.Focus();
                return false;
            }
            if (!string.IsNullOrWhiteSpace(TxtPrice2.Text) && (!decimal.TryParse(TxtPrice2.Text, out decimal hrg2) || hrg2 < 0))
            {
                errorMessage = "Harga Usulan harus berupa angka desimal positif!";
                TxtPrice2.Focus();
                return false;
            }
            if (!string.IsNullOrWhiteSpace(TxtFrac.Text) && (!int.TryParse(TxtFrac.Text, out int frc) || frc < 0))
            {
                errorMessage = "Frac / Unit harus berupa angka bulat positif!";
                TxtFrac.Focus();
                return false;
            }
            if (TxtName.Text.Contains("'") || TxtName.Text.Contains("--"))
            {
                errorMessage = "Nama Barang tidak boleh mengandung karakter petik (') atau double strip (--)!";
                TxtName.Focus();
                return false;
            }

            return true;
        }
        private Product MapFormToModel()
        {
            decimal.TryParse(TxtPrice.Text, out decimal hrgJual);
            decimal.TryParse(TxtPrice2.Text, out decimal hrgJual2);
            if (!int.TryParse(TxtFrac.Text, out int frac)) frac = 1;

            return new Product
            {
                PRD_PRDCD = TxtCode.Text.Trim(),
                PRD_NAMA = TxtName.Text.Trim(),
                PRD_MERK = TxtMerk.Text.Trim(),
                PRD_FLAVOUR = TxtFlavour.Text.Trim(),
                PRD_KEMASAN = TxtKemasan.Text.Trim(),
                PRD_SIZE = TxtSize.Text.Trim(),
                PRD_DESKRIPSI = TxtDeskripsi.Text.Trim(),
                PRD_UNIT = string.IsNullOrWhiteSpace(TxtUnit.Text) ? "PCS" : TxtUnit.Text.Trim(),
                PRD_FRAC = frac,
                PRD_HRGJUAL = hrgJual,
                PRD_HRGJUAL2 = hrgJual2
            };
        }

        // --- EVENT HANDLERS ---
        private void GridData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridProduct.GridData.SelectedItem == null || !(GridProduct.GridData.SelectedItem is DataRowView selectedRow)) return;

            try
            {
                string colCode = selectedRow.Row.Table.Columns.Contains("PRD_PRDCD") ? "PRD_PRDCD" : "Kode Produk";
                string prdCd = selectedRow[colCode].ToString();
                if (string.IsNullOrEmpty(prdCd)) return;

                DataRow detail = _controller.GetProductDetail(prdCd);
                if (detail != null)
                {
                    TxtCode.Text = detail["PRD_PRDCD"].ToString();
                    TxtName.Text = detail["PRD_NAMA"].ToString();
                    TxtMerk.Text = detail["PRD_MERK"].ToString();
                    TxtFlavour.Text = detail["PRD_FLAVOUR"].ToString();
                    TxtKemasan.Text = detail["PRD_KEMASAN"].ToString();
                    TxtSize.Text = detail["PRD_SIZE"].ToString();
                    TxtDeskripsi.Text = detail["PRD_DESKRIPSI"].ToString();
                    TxtUnit.Text = detail["PRD_UNIT"].ToString();
                    TxtFrac.Text = detail["PRD_FRAC"].ToString();
                    TxtPrice.Text = detail["PRD_HRGJUAL"].ToString();
                    TxtPrice2.Text = detail["PRD_HRGJUAL2"].ToString();
                    TxtCode.IsEnabled = false;
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Error: " + ex.Message); }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm(out string errValidation))
            {
                ErrorHelper.ShowValidationError(errValidation);
                return;
            }

            Product p = MapFormToModel();
            if (_controller.InsertProduct(p, out string dbErr))
            {
                RefreshData();
                ClearForm();
                MessageBox.Show("Produk berhasil ditambahkan!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else MessageBox.Show("Gagal simpan: " + dbErr, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm(out string errValidation))
            {
                ErrorHelper.ShowValidationError(errValidation);
                return;
            }

            Product p = MapFormToModel();
            if (_controller.UpdateProduct(p, out string dbErr))
            {
                RefreshData();
                ClearForm();
                MessageBox.Show("Produk berhasil diubah!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else MessageBox.Show("Gagal update: " + dbErr, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtCode.Text)) return;

            if (MessageBox.Show("Hapus data produk ini?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (_controller.DeleteProduct(TxtCode.Text, out string err))
                {
                    RefreshData();
                    ClearForm();
                }
            }
        }

        // --- SEARCH EVENT LOGIC ---
        private void BtnSearch_Click(object sender, RoutedEventArgs e) => ExecuteSearch();
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) ExecuteSearch(); }

        private void ExecuteSearch()
        {
            string keyword = TxtSearch.Text == PLACEHOLDER ? "" : TxtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                RefreshData();
                return;
            }
            string searchBy = (CmbSearchType.SelectedItem as ComboBoxItem)?.Tag.ToString() ?? "PRD_PRDCD";
            GridProduct.SetDataSource(_controller.SearchProducts(searchBy, keyword));
        }

        private void TxtSearch_GotFocus(object sender, RoutedEventArgs e) { if (TxtSearch.Text == PLACEHOLDER) { TxtSearch.Text = ""; TxtSearch.Foreground = System.Windows.Media.Brushes.Black; } }
        private void TxtSearch_LostFocus(object sender, RoutedEventArgs e) { if (string.IsNullOrWhiteSpace(TxtSearch.Text)) InitializeSearchBox(); }
        private void BtnClear_Click(object sender, RoutedEventArgs e) => ClearForm();

        private void ClearForm()
        {
            TxtCode.Clear(); TxtName.Clear(); TxtMerk.Clear(); TxtFlavour.Clear();
            TxtKemasan.Clear(); TxtSize.Clear(); TxtDeskripsi.Clear(); TxtPrice.Clear(); TxtPrice2.Clear();
            TxtUnit.Text = "PCS"; TxtFrac.Text = "1"; TxtCode.IsEnabled = true;
            RefreshData();
        }
    }
}