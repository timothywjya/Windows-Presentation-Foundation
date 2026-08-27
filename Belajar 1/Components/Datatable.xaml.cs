using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Belajar_1.Components
{
    public partial class Datatable : UserControl
    {
        // --- Dukungan binding MVVM (ditambahkan untuk FormUser) ---------------
        // Form yang belum dimigrasi ke MVVM (Produk/Supplier/Retur) tetap
        // memakai SetDataSource(DataTable) di bawah seperti sebelumnya, jadi
        // penambahan ini tidak mengubah perilaku form-form tersebut.

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(Datatable),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable? ItemsSource
        {
            get => (IEnumerable?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(Datatable),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object? SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        // True hanya ketika grid diisi lewat ItemsSource (binding MVVM ke
        // koleksi objek C#, mis. ObservableCollection<User>). Form lama yang
        // memanggil SetDataSource(DataTable) membiarkan ini tetap false,
        // supaya header/caption kolom mereka yang sudah diatur lewat alias
        // SQL tidak diutak-atik oleh logika di bawah.
        private bool _isBoundToObjectCollection;

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (Datatable)d;
            self._isBoundToObjectCollection = true;
            self.GridData.ItemsSource = (IEnumerable?)e.NewValue;
        }

        // -----------------------------------------------------------------------

        public Datatable()
        {
            InitializeComponent();

            GridData.SelectionChanged += (s, e) => SelectedItem = GridData.SelectedItem;

            GridData.AutoGeneratingColumn += (s, e) =>
            {
                if (!_isBoundToObjectCollection) return;

                // Jangan pernah menampilkan kolom Password apa pun objek yang di-bind.
                if (e.PropertyName.Equals("Password", System.StringComparison.OrdinalIgnoreCase))
                {
                    e.Cancel = true;
                    return;
                }

                // "UserLevel" -> "User Level" biar header lebih enak dibaca.
                e.Column.Header = Regex.Replace(e.PropertyName, "(?<!^)([A-Z])", " $1");
            };
        }

        /// <summary>
        /// Dipakai oleh form-form lama (Produk/Supplier/Retur) yang masih
        /// berbasis DataTable dan belum dimigrasi ke MVVM. Tidak diubah agar
        /// form-form tersebut tetap berjalan persis seperti sebelumnya.
        /// </summary>
        public void SetDataSource(DataTable dt)
        {
            _isBoundToObjectCollection = false;
            GridData.ItemsSource = dt.DefaultView;
        }
    }
}