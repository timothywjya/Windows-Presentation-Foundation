using System.Collections;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Belajar_1.Components
{
    public partial class Datatable : UserControl
    {
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

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (Datatable)d;
            self.GridData.ItemsSource = (IEnumerable?)e.NewValue;
        }

        public Datatable()
        {
            InitializeComponent();

            GridData.SelectionChanged += (s, e) => SelectedItem = GridData.SelectedItem;

            GridData.AutoGeneratingColumn += (s, e) =>
            {
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
    }
}
