﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
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
using System.Windows.Threading;

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Random random = new Random();
        DispatcherTimer requestTimer = new DispatcherTimer();
        public Window1()
        {
            InitializeComponent();

            requestTimer.Tick += request;
            requestTimer.Interval = TimeSpan.FromSeconds(3);
            requestTimer.Start();
        }

        async void request(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("http://192.168.147.53:4914/PersonLocations");
            string responseBody = await response.Content.ReadAsStringAsync();
            List<PersonLocation> personsLocation = JsonConvert.DeserializeObject<JsonRoot>(responseBody).response;
            humansGrid.Children.Clear();
            foreach (var personLocation in personsLocation)
            {
                createHuman(personLocation);
            }
        }

        void createHuman(PersonLocation personLocation)
        {
            Canvas currentRoom;
            if (personLocation.LastSecurityPointDirection == "in")
                currentRoom = (Canvas)humansGrid.FindName("skud" + personLocation.lastSecurityPointNumber);
            else if (personLocation.lastSecurityPointNumber != 0 || personLocation.lastSecurityPointNumber != 1)
                currentRoom = skud0;
            else
                return;
            double width = currentRoom.Width;
            double height = currentRoom.Height;
            Thickness margins = currentRoom.Margin;

            int radius = 5;
            int yMin = (int)margins.Bottom + radius + 5;
            int yMax = (int)margins.Bottom - radius - 5 + (int)height;
            int xMin = (int)margins.Left + radius + 5;
            int xMax = (int)margins.Left - radius - 5 + (int)width;
            int x;
            int y;
            do
            {
                x = random.Next(xMin, xMax);
                y = random.Next(yMin, yMax);
            }
            while (isAnyoneAround(x, y, radius));
            Ellipse ellipse = new Ellipse();
            ellipse.Width = radius * 2;
            ellipse.Height = radius * 2;
            ellipse.VerticalAlignment = VerticalAlignment.Bottom;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            if (personLocation.personRole == "Сотрудник")
                ellipse.Fill = Brushes.Blue;
            else
                ellipse.Fill = Brushes.Green;
            ellipse.Margin = new Thickness(x, 0, 0, y);
            humansGrid.Children.Add(ellipse);
        }

        bool isAnyoneAround(int x,int y, int radius)
        {
            var humans = humansGrid.Children;
            foreach (Ellipse human in humans)
            {
                double gip = Math.Sqrt(Math.Pow(Math.Abs(x - human.Margin.Left), 2) + Math.Pow(Math.Abs(y - human.Margin.Bottom), 2));
                if (gip <= radius * 2)
                {
                    return true;
                }
            }
            return false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            requestTimer.Stop();
        }
    }

    public class PersonLocation
    {
        public int personCode { get; set; }
        public string personRole { get; set; }
        public int lastSecurityPointNumber { get; set; }
        public string LastSecurityPointDirection { get; set; }
        public string lastSecurityPointTime { get; set; }
    }
    public class JsonRoot
    {
        public List<PersonLocation> response { get; set; }
    }
}
