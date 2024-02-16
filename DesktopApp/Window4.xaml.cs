using DataBaseClasses;
using static DataBaseClasses.DBInteract;
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
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        AppointmentInfo appointmentInfo;

        public Window4(Client client)
        {
            InitializeComponent();
            appointmentInfo = new AppointmentInfo()
            {
                client = client
            };

            UpdateList();
        }

        private void dat_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            cl a = dataGrid.Items[dataGrid.Items.Count - 1] as cl;
            MessageBox.Show(a.ToString() + "\n" + sender.ToString() + "\ndat_RowEditEnding");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new testWindow();
            if(dialog.ShowDialog() == true)
            {
                Medicament? medicament = db.Medicaments.Where(x => x.medicamentName == dialog.MedicamentName).First();
                if(medicament == null)
                {
                    medicament = new Medicament() 
                    {
                        medicamentName = dialog.MedicamentName
                    };
                    db.Medicaments.Add(medicament);
                    db.SaveChanges();
                    medicament = db.Medicaments.Where(x => x.medicamentName == dialog.MedicamentName).First();
                }

                Prescription prescription = new Prescription()
                {
                    medicament = medicament,
                    dose = dialog.Dose,
                    format = dialog.Format,
                };

                db.prescriptions.Add(prescription);
                db.SaveChanges();
            }
        }

        void UpdateList()
        {
            prescriptionList.ItemsSource = db.prescriptions.Where(x => x.appointmentInfo.appointmentInfo_id == appointmentInfo.appointmentInfo_id).ToList();
        }
    }

    class cl
    {
        public string medicamentName { get; set; }
        public string dose { get; set; }
        public string format { get; set; }
    }
}
