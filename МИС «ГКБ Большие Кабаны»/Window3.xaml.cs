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
using static МИС__ГКБ_Большие_Кабаны_.DBInteract;

namespace МИС__ГКБ_Большие_Кабаны_
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        List<WeekTimetablee> weekTimetableList;

        List<WeekStringTimeTable> weekStringTimeTableList;

        public Window3()
        {
            InitializeComponent();

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
    }

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
    }
}
