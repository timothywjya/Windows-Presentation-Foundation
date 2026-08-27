using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Belajar_1.Controller;

namespace Belajar_1.Components.Form
{
    public partial class FormSupplier : UserControl
    {
        private SupplierController _controller = new SupplierController();

        public FormSupplier()
        {
            InitializeComponent();
            RefreshData();
            GridSupplier.GridData.SelectionChanged += GridData_SelectionChanged;
        }

        public void RefreshData()
        {
            GridSupplier.SetDataSource(_controller.GetAllSuppliers());
        }

        private void GridData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridSupplier.GridData.SelectedItem is DataRowView row)
            {
                TxtSuppCode.Text = row["Kode Supplier"].ToString();
                TxtSuppName.Text = row["Nama Supplier"].ToString();
                TxtAlias.Text = row["Singkatan"].ToString();
                TxtPhone.Text = row["Telepon"].ToString();
                TxtCP.Text = row["Contact Person"].ToString();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.InsertSupplier(TxtSuppCode.Text, TxtSuppName.Text, TxtAlias.Text, TxtPhone.Text, TxtCP.Text, out string err))
            {
                RefreshData();
                ClearForm();
            }
            else MessageBox.Show(err);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.UpdateSupplier(TxtSuppCode.Text, TxtSuppName.Text, TxtAlias.Text, TxtPhone.Text, TxtCP.Text, out string err))
            {
                RefreshData();
                ClearForm();
            }
            else MessageBox.Show(err);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.DeleteSupplier(TxtSuppCode.Text, out string err))
            {
                RefreshData();
                ClearForm();
            }
            else MessageBox.Show(err);
        }

        private void ClearForm()
        {
            TxtSuppCode.Clear();
            TxtSuppName.Clear();
            TxtAlias.Clear();
            TxtPhone.Clear();
            TxtCP.Clear();
        }
    }
}