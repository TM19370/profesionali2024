using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Random random = new Random();
        public Window1()
        {
            InitializeComponent();



        }

        List<Human> humansList = new List<Human>();
        int c = 1;

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Canvas currentCanvas = (Canvas)mg.FindName("skud16");
            Thickness margins = currentCanvas.Margin;

            if (e.ChangedButton == MouseButton.Right)//delite human
            {
                Human curPos = humansList.Where(a => a.code == Convert.ToInt32(textbox.Text)).First();
                humansList.Remove(curPos);
                mg.Children.Remove(curPos.ellipse);
                return;
            }
            eg.Children.Clear();
            int radius = 5;
            int x = random.Next(5, 111);
            int y = random.Next(5, 173);
            while (isAnyoneAround(x, y, radius))
            {
                Ellipse errorEllipse = new Ellipse();
                errorEllipse.Width = radius * 2;
                errorEllipse.Height = radius * 2;
                errorEllipse.VerticalAlignment = VerticalAlignment.Bottom;
                errorEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                errorEllipse.Fill = Brushes.Red;
                errorEllipse.Margin = new Thickness(x, 0, 0, y);
                eg.Children.Add(errorEllipse);

                x = random.Next(5, 111);
                y = random.Next(5, 174);
            }
            Ellipse ellipse = new Ellipse();
            ellipse.Width = radius * 2;
            ellipse.Height = radius * 2;
            ellipse.VerticalAlignment = VerticalAlignment.Bottom;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.Fill = Brushes.Blue;
            ellipse.Margin = new Thickness(x, 0, 0, y);
            mg.Children.Add(ellipse);
            humansList.Add(new Human { code = 1, ellipse = ellipse, point = new Point(x, y) });
            c++;
        }

        /*
         x = random.Next(rooms[personCode].roomStartX, rooms[personCode].roomEndX);
         y = random.Next(rooms[personCode].roomStartY, rooms[personCode].roomEndY);
        */


        List<Room> rooms = new List<Room>();

        void createHuman(PersonLocation personLocation)
        {
            Canvas currentRoom = (Canvas)mg.FindName("skud" + personLocation.lastSecurityPointNumber);
            double width = currentRoom.Width;
            double height = currentRoom.Height;
            Thickness margins = currentRoom.Margin;
            int personCode = personLocation.personCode;

            int radius = 5;
            int x = random.Next((int)margins.Left + 5,(int)margins.Left - 5 + (int)width);
            int y = random.Next((int)margins.Bottom + 5, (int)margins.Bottom - 5 + (int)height);
            while (isAnyoneAround(x, y, radius))
            {
                x = random.Next(rooms[personCode].roomStartX, rooms[personCode].roomEndX);
                y = random.Next(rooms[personCode].roomStartY, rooms[personCode].roomEndY);
            }
            Ellipse ellipse = new Ellipse();
            ellipse.Width = radius * 2;
            ellipse.Height = radius * 2;
            ellipse.VerticalAlignment = VerticalAlignment.Bottom;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.Fill = Brushes.Blue;
            ellipse.Margin = new Thickness(x, 0, 0, y);
            mg.Children.Add(ellipse);
            humansList.Add(new Human { code = personLocation.personCode, ellipse = ellipse, point = new Point(x, y) });
        }

        void deliteHuman(PersonLocation personLocation)
        {
            Human curPos = humansList.Where(x => x.code == personLocation.personCode).First();
            humansList.Remove(curPos);
            mg.Children.Remove(curPos.ellipse);
        }

        bool isAnyoneAround(int x,int y, int radius)
        {
            var aroundList = new List<Human>();
            foreach (var human in humansList)
            {
                double gip = Math.Sqrt(Math.Pow(Math.Abs(x - human.point.X), 2) + Math.Pow(Math.Abs(y - human.point.Y), 2));
                if(gip <= radius * 2)
                {
                    aroundList.Add(human);
                }
            }
            if (aroundList.Count > 0)
                return true;
            else
                return false;
        }
    }

    class Room
    {
        public int roomStartX {  get; set; }
        public int roomEndX { get; set; }
        public int roomStartY { get; set; }
        public int roomEndY { get; set; }
    }

    class PersonLocation
    {
        public int personCode { get; set; }
        public string personRole { get; set; }
        public string lastSecurityPointNumber { get; set; }
        public string lastSecurityPointTime { get; set; }
    }

    class Human
    {
        public int code { get; set; }
        public Ellipse ellipse { get; set; }
        public Point point { get; set; }
    }
}
