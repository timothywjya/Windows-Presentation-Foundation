using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Belajar_1.Controller;

namespace Belajar_1.Components.Form
{
    public partial class FormUser : UserControl
    {
        private UserController _db = new UserController();

        public FormUser()
        {
            InitializeComponent();
            RefreshData();

            // Event sinkronisasi klik baris tabel
            GridUser.GridData.SelectionChanged += GridUser_SelectionChanged;
        }

        public void RefreshData()
        {
            GridUser.SetDataSource(_db.GetAllUsers());
        }

        private void GridUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridUser.GridData.SelectedItem is DataRowView row)
            {
                TxtUserId.Text = row["User ID"].ToString();
                TxtUsername.Text = row["Nama User"].ToString();
                TxtEmail.Text = row["Email"].ToString();
                TxtLevel.Text = row["Level"].ToString();
                TxtUserId.IsEnabled = false; // Kunci ID saat mode edit
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_db.InsertUser(TxtUserId.Text, TxtPassword.Password, TxtUsername.Text, TxtEmail.Text, int.Parse(TxtLevel.Text), out string err))
            {
                RefreshData();
                BtnClear_Click(null, null);
            }
            else MessageBox.Show("Error: " + err);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_db.UpdateUser(TxtUserId.Text, TxtPassword.Password, TxtUsername.Text, TxtEmail.Text, int.Parse(TxtLevel.Text), out string err))
            {
                RefreshData();
                BtnClear_Click(null, null);
            }
            else MessageBox.Show("Error: " + err);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Hapus User ini?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (_db.DeleteUser(TxtUserId.Text, out string err))
                {
                    RefreshData();
                    BtnClear_Click(null, null);
                }
                else MessageBox.Show(err);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxtUserId.Clear();
            TxtUsername.Clear();
            TxtEmail.Clear();
            TxtPassword.Clear();
            TxtLevel.Clear();
            TxtUserId.IsEnabled = true;
        }
    }
}