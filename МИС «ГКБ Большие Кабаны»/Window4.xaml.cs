using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace МИС__ГКБ_Большие_Кабаны_
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public Window4()
        {
            InitializeComponent();

            dat.ItemsSource = new List<cl> { new cl { medicamentName = "tst", dose = "1", format = "asdasda" } };
        }

        private void dat_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            cl a = dataGrid.Items[dataGrid.Items.Count - 1] as cl;
            MessageBox.Show(a.ToString() + "\n" + sender.ToString() + "\ndat_RowEditEnding");
        }
    }

    class cl
    {
        public string medicamentName { get; set; }
        public string dose { get; set; }
        public string format { get; set; }
    }
}
