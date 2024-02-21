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
    {/*
        List<WeekTimetablee> weekTimetableList;

        List<WeekStringTimeTable> weekStringTimeTableList = new List<WeekStringTimeTable>();
        */
        public Window3()
        {
            InitializeComponent();

            List<tst> tstList = new List<tst>();

            for (int i = 0; i < 10; i++)
            {
                tstList.Add(new tst()
                {
                    doctorFullName = "Иванов Иван Иванович",
                    doc = new List<doc>()
                    {
                        new doc() {
                            dayOfWeek = DayOfWeek.Monday,
                            workTime = "12:00 - 14:00",
                            zapisi = new List<Zapisi>()
                            {
                                new Zapisi()
                                {
                                    time = "12:00 - 12:10",
                                    clientFullName = "Данилов Данил Данилович"
                                },
                                new Zapisi()
                                {
                                    time = "12:10 - 12:20",
                                    clientFullName = "Егоров Егор Егорович"
                                }
                            }
                        },
                        new doc() {
                            dayOfWeek = DayOfWeek.Tuesday,
                            workTime = "12:00 - 14:00",
                            zapisi = new List<Zapisi>()
                            {
                                new Zapisi()
                                {
                                    time = "13:30 - 13:40",
                                    clientFullName = "Денисов Денис Денисович"
                                }
                            }
                        }
                    }
                });
            }


            mainList.ItemsSource = tstList;

            /*
            List<Doctor> doctors = db.doctors.ToList();

            foreach (Doctor doctor in doctors)
            {
                WeekTimetable weekTimetable = db.weekTimetable.Where(x => x.doctor.doctor_id == doctor.doctor_id).First();
                List<Timetable> timetables = db.timetables.Where(x => x.weekTimetable.weekTimeTable_id == weekTimetable.weekTimeTable_id).ToList();

                WeekStringTimeTable weekStringTimeTable = new WeekStringTimeTable() 
                { 
                    doctorFullName = doctor.GetFullName(),
                    monday = timetables.Where(x => x.dayOfWeek == DayOfWeek.Monday).First().GetAsString(),
                    tuesday = timetables.Where(x => x.dayOfWeek == DayOfWeek.Tuesday).First().GetAsString(),
                    wednesday = timetables.Where(x => x.dayOfWeek == DayOfWeek.Wednesday).First().GetAsString(),
                    thursday = timetables.Where(x => x.dayOfWeek == DayOfWeek.Thursday).First().GetAsString(),
                    friday = timetables.Where(x => x.dayOfWeek == DayOfWeek.Friday).First().GetAsString(),
                    saturday = timetables.Where(x => x.dayOfWeek == DayOfWeek.Saturday).First().GetAsString()
                };

                weekStringTimeTableList.Add(weekStringTimeTable);
            }

            dataGrid.ItemsSource = weekStringTimeTableList;
            */
            /*
            List<Doctor> doctors = db.doctors.ToList();

            dg.ItemsSource = doctors;

            List<Timetable> timetables = db.timetables.ToList();
            /*
            foreach (Doctor doctor in doctors)
            {
                List<Timetable> doctorsTimetable = timetables.Where(x => x.doctor == doctor).ToList();
                
                WeekTimetablee weekTimetable = new WeekTimetablee() 
                {
                    doctor = doctor,
                    cabinet = doctorsTimetable[0].cabinet,
                    monday = doctorsTimetable.Where(x => x.dayOfWeek == DayOfWeek.Monday).First(),
                    tuesday = doctorsTimetable.Where(x => x.dayOfWeek == DayOfWeek.Tuesday).First(),
                    wednesday = doctorsTimetable.Where(x => x.dayOfWeek == DayOfWeek.Wednesday).First(),
                    thursday = doctorsTimetable.Where(x => x.dayOfWeek == DayOfWeek.Tuesday).First(),
                    friday = doctorsTimetable.Where(x => x.dayOfWeek == DayOfWeek.Friday).First(),
                    saturday = doctorsTimetable.Where(x => x.dayOfWeek == DayOfWeek.Sunday).First(),
                };

                weekTimetableList.Add(weekTimetable);
            }
            *

            qwe.ItemsSource = timetables;*/
        }

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

                client = GetClientByID(450).Result;
                MessageBox.Show(client.FullName);
            }
            catch (Exception ex) 
            {

            }
        }
    }

    public class tst
    {
        public string doctorFullName { get; set; }
        public virtual List<doc> doc { get; set; }
    }

    public class doc
    {
        public DayOfWeek dayOfWeek { get; set; }
        public string workTime { get; set; }
        public virtual List<Zapisi> zapisi { get; set; }
    }

    public class Zapisi
    {
        public string time { get; set; }
        public string clientFullName { get; set; }
    }
    /*
    public class WeekStringTimeTable
    {
        public string doctorFullName { get; set; }
        public string monday { get; set; }
        public string tuesday { get; set; }
        public string wednesday { get; set; }
        public string thursday { get; set; }
        public string friday { get; set; }
        public string saturday { get; set; }
    }

    public class WeekTimetablee
    {
        public virtual Doctor doctor { get; set; }
        public string cabinet { get; set; }
        public Timetable monday { get; set; }
        public Timetable tuesday { get; set; }
        public Timetable wednesday { get; set; }
        public Timetable thursday { get; set; }
        public Timetable friday { get; set; }
        public Timetable saturday { get; set; }
    }*/
}
