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

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для testWindow.xaml
    /// </summary>
    public partial class testWindow : Window
    {
        public testWindow()
        {
            InitializeComponent();
        }

        public string MedicamentName { get { return medicamentNameTextBox.Text; } set { medicamentNameTextBox.Text = value; } }
        public double Dose { get { return Convert.ToDouble(doseTextBox.Text); } set { doseTextBox.Text = value.ToString(); } }
        public string Format { get { return formatTextBox.Text; } set { formatTextBox.Text = value; } }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
