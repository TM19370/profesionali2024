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
using DataBaseClasses;
using static DataBaseClasses.DBInteract;

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();

            List<Bed> beds = db.Beds.ToList();
            foreach (var bed in beds)
            {
                if(bed.Client != null)
                {
                    string name = (bed.bedCode + bed.roomNumber).ToString();
                    Canvas bedCanvas = (Canvas)mg.FindName(name);
                    int radius = 7;
                    Ellipse ellipse = new Ellipse();
                    ellipse.Height = radius * 2;
                    ellipse.Width = radius * 2;
                    ellipse.Fill = Brushes.Green;
                    ellipse.VerticalAlignment = VerticalAlignment.Bottom;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    Thickness margins = new Thickness();
                    margins.Left = bedCanvas.Margin.Left + bedCanvas.Width / 2 - radius;
                    margins.Bottom = bedCanvas.Margin.Bottom + bedCanvas.Height / 2 - radius;
                    ellipse.Margin = margins;
                    ellipse.Name = "h" + name;
                    ellipse.MouseMove += human_MouseMove;
                    mg.Children.Add(ellipse);
                }
            }
        }

        private void human_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Ellipse ellipse = (Ellipse)sender;
                DragDrop.DoDragDrop(ellipse, new DataObject(DataFormats.Serializable, ellipse), DragDropEffects.Move);
            }
        }

        private void human_Drop(object sender, DragEventArgs e)
        {
            Canvas newBedCanvas = (Canvas)sender;
            int newRoomNumber = Convert.ToInt32(newBedCanvas.Name[1].ToString() + newBedCanvas.Name[2].ToString() + newBedCanvas.Name[3].ToString());
            string newBedCode = newBedCanvas.Name[0].ToString();
            Bed newBed = db.Beds.Where(x => x.roomNumber == newRoomNumber && x.bedCode == newBedCode).First();
            if (newBed.Client != null)
            {
                MessageBox.Show("Койка занята");
                return;
            }

            Ellipse human = (Ellipse)e.Data.GetData(DataFormats.Serializable);
            int roomNumber = Convert.ToInt32(human.Name[2].ToString() + human.Name[3].ToString() + human.Name[4].ToString());
            string bedCode = human.Name[1].ToString();
            Bed currentBed = db.Beds.Where(x => x.roomNumber == roomNumber && x.bedCode == bedCode).First();
            newBed.EditClient(currentBed.Client);
            currentBed.EditClient(null);

            Thickness margins = new Thickness();
            margins.Left = newBedCanvas.Margin.Left + newBedCanvas.Width / 2 - human.Width / 2;
            margins.Bottom = newBedCanvas.Margin.Bottom + newBedCanvas.Height / 2 - human.Width / 2;
            human.Margin = margins;

            db.SaveChanges();
        }

        private void Discharge_Drop(object sender, DragEventArgs e)
        {
            Ellipse human = (Ellipse)e.Data.GetData(DataFormats.Serializable);
            int roomNumber = Convert.ToInt32(human.Name[2].ToString() + human.Name[3].ToString() + human.Name[4].ToString());
            string bedCode = human.Name[1].ToString();
            Bed currentBed = db.Beds.Where(x => x.roomNumber == roomNumber && x.bedCode == bedCode).First();
            currentBed.EditClient(null);
            db.SaveChanges();
            mg.Children.Remove(human);
        }
    }
}
