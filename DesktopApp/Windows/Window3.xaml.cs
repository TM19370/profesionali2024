using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
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
using Newtonsoft.Json;
using static DataBaseClasses.DBInteract;

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();

            //asd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF"); цвета для отмечания событий

            List<WeekSchedule> weekSchedules = new List<WeekSchedule>();

            List<Account> doctors = new List<Account>();
            foreach (Account doctor in doctors)
            {
                List<ScheduleElement> scheduleElements = db.scheduleElements.Where(x => x.account.account_id == doctor.account_id).ToList();

                string pn = "";
                string vt = "";
                string sr = "";
                string ct = "";
                string pt = "";
                string sb = "";

                foreach (var scheduleElement in scheduleElements)
                {
                    List<Appointment> appointments = db.appointments.Where(x => x.date == scheduleElement.date).ToList();
                    foreach (Appointment appointment in appointments)
                    {
                        switch (appointment.date.DayOfWeek)
                        {
                            case System.DayOfWeek.Monday:
                                pn += $"{appointment.GetTimeAsString}\n";
                                break;
                            case System.DayOfWeek.Tuesday:
                                vt += $"{appointment.GetTimeAsString}\n";
                                break;
                            case System.DayOfWeek.Wednesday:
                                sr += $"{appointment.GetTimeAsString}\n";
                                break;
                            case System.DayOfWeek.Thursday:
                                ct += $"{appointment.GetTimeAsString}\n";
                                break;
                            case System.DayOfWeek.Friday:
                                pt += $"{appointment.GetTimeAsString}\n";
                                break;
                            case System.DayOfWeek.Saturday:
                                sb += $"{appointment.GetTimeAsString}\n";
                                break;
                        }
                    }
                }

                weekSchedules.Add(new WeekSchedule
                {
                    fullName = scheduleElements[0].account.FullName.GetFullName,
                    pn = pn,
                    vt = vt,
                    sr = sr,
                    ct = ct,
                    pt = pt,
                    sb = sb
                });
            }




            mainGrid.ItemsSource = weekSchedules;
        }

        /// тестовая функция
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://192.168.147.66:5120/Client/450");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                Client client;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    var json = await responseContent.ReadAsStringAsync();
                    client = JsonConvert.DeserializeObject<Client>(json);
                    //DisplayAlert("", client.FullName, "Ok");
                }

                //client = GetClientByID(450).Result;
                //MessageBox.Show(client.FullName);
            }
            catch (Exception ex) 
            {

            }
        }
        //////////////////////
    }

    public class WeekSchedule
    {
        public string fullName { get; set; }
        public string pn { get; set; }
        public string vt { get; set; }
        public string sr { get; set; }
        public string ct { get; set; }
        public string pt { get; set; }
        public string sb { get; set; }
    }
}
