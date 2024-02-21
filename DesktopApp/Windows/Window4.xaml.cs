using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using static DesktopApp.ApiInteract;
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
using System.Net.Http;

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        List<Prescription> prescriptions = new List<Prescription>();
        public Window4()
        {
            InitializeComponent();
        }

        private void AddPrescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (prescriptionList.Items.Count == 10)
            {
                MessageBox.Show("Рецептов не может быть больше десяти", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var dialog = new AddPrescriptionDialogWindow();
            if(dialog.ShowDialog() == true)
            {
                if (dialog.MedicamentName == "" || dialog.Dose == null || dialog.Format == "")
                {
                    MessageBox.Show("Одно из введенных значений было пустое или имело некоректный формат, добавление рецепта отменено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Medicament medicament = new Medicament()
                {
                    medicamentName = dialog.MedicamentName
                };

                Prescription prescription = new Prescription()
                {
                    medicament = medicament,
                    dose = (double)dialog.Dose,
                    format = dialog.Format,
                };

                prescriptionList.Items.Add(prescription);
                prescriptions.Add(prescription);
            }
        }

        private void DeletePrescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (prescriptionList.SelectedItem == null)
            {
                MessageBox.Show("Выберите рецепт для удаления", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            prescriptionList.Items.Remove(prescriptionList.SelectedItem);
            prescriptions.Remove(prescriptionList.SelectedItem as Prescription);
        }
        
        private async void addAppointmentInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Client client = await SendRequest<Client>("/Client/" + clientIdTextBox.Text, HttpMethod.Get, null);
            if (client == default)
                return;

            AppointmentInfo appointmentInfo = new AppointmentInfo()
            {
                client = client,
                anamnesis = anamnesisTextBox.Text,
                symptoms = symptomsTextBox.Text,
                diagnosis = diagnosisTextBox.Text,
                recommendations = recommendationsTextBox.Text
            };
            appointmentInfo = await SendRequest<AppointmentInfo>("/AppointmentInfo", HttpMethod.Post, appointmentInfo);
            if (appointmentInfo == default)
                return;

            foreach (Prescription prescription in prescriptions)
            {
                Medicament medicament = prescription.medicament;
                medicament = await SendRequest<Medicament>("/Medicament", HttpMethod.Post, medicament);
                if (medicament == default)
                    return;

                prescription.medicament = medicament;
                prescription.appointmentInfo = appointmentInfo;

                Prescription newPrescription = await SendRequest<Prescription>("/Prescription", HttpMethod.Post, prescription);
                if (newPrescription == default)
                    return;
            }

            MessageBox.Show("Информация загружена", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //сделать возвращение назад
        }
    }
}
