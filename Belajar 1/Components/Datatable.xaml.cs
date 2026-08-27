using System.Data;
using System.Windows.Controls;

namespace Belajar_1.Components
{
    public partial class Datatable : UserControl
    {
        public Datatable()
        {
            InitializeComponent();
        }

        public void SetDataSource(DataTable dt)
        {
            GridData.ItemsSource = dt.DefaultView;
        }
    }
}