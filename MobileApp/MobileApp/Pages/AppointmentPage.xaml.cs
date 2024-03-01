using Android.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentPage : ContentPage
    {
        public AppointmentPage()
        {
            InitializeComponent();
        }

        public bool isAudioMessageExsist = false;

        private async Task ShowError(string massege)
        {
            await DisplayAlert("Ошибка", massege, "Ok");
        }

        private async Task<T> SendRequest<T>(string url, HttpMethod httpMethod, T content)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://192.168.147.66:5120{url}");
            request.Method = httpMethod;
            request.Headers.Add("Accept", "application/json");
            if (httpMethod == HttpMethod.Post)
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(content));
                request.Content = stringContent;
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            HttpResponseMessage response = await httpClient.SendAsync(request);
            HttpContent responseContent = response.Content;
            string json = await responseContent.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ErrorResponce errorResponce = JsonConvert.DeserializeObject<ErrorResponce>(json);
                await ShowError(errorResponce.error);
                return default;
            }

            T responseObject = JsonConvert.DeserializeObject<T>(json);
            return responseObject;
        }

        private async Task<string> SendRecord()
        {
            // адрес сервера
            var serverAddress = "http://192.168.147.66:5120/appointment/audio";
            // пусть к файлу
            var filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)}/record.3gp";

            // создаем MultipartFormDataContent
            var multipartFormContent = new MultipartFormDataContent();
            // Загружаем отправляемый файл
            var fileStreamContent = new StreamContent(System.IO.File.OpenRead(filePath));
            // Устанавливаем заголовок Content-Type
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/3gpp");
            // Добавляем загруженный файл в MultipartFormDataContent
            multipartFormContent.Add(fileStreamContent, name: "file", fileName: "record.3gp");

            // Отправляем файл
            var response = await httpClient.PostAsync(serverAddress, multipartFormContent);
            // считываем ответ
            var responseText = await response.Content.ReadAsStringAsync();

            return responseText;
        }

        private static HttpClient httpClient = new HttpClient();

        private async void addAppointmentInfoButton_Clicked(object sender, EventArgs e)
        {
            Client client = await SendRequest<Client>("/clients/" + clientIdEntry.Text, HttpMethod.Get, null);
            if (client == default)
                return;

            string audioMessageFileName = null;
            if (isAudioMessageExsist)
            {
                audioMessageFileName = await SendRecord();
            }

            AppointmentInfo appointmentInfo = new AppointmentInfo()
            {
                client = client,
                anamnesis = anamnesisEditor.Text,
                symptoms = symptomsEditor.Text,
                diagnosis = diagnosisEditor.Text,
                recommendations = recommendationsEditor.Text,
                audioMessageFileName = audioMessageFileName
            };
            appointmentInfo = await SendRequest<AppointmentInfo>("/appointment/info/create", HttpMethod.Post, appointmentInfo);
            if (appointmentInfo == default)
                return;

            List<Prescription> prescriptions = new List<Prescription>();
            prescriptions = prescriptionListView.ItemsSource as List<Prescription>;

            foreach (Prescription prescription in prescriptions)
            {
                Medicament medicament = prescription.medicament;
                medicament = await SendRequest<Medicament>("/appointment/medicament", HttpMethod.Post, medicament);
                if (medicament == default)
                    return;

                prescription.medicament = medicament;
                prescription.appointmentInfo = appointmentInfo;

                Prescription newPrescription = await SendRequest<Prescription>("/appointment/prescription", HttpMethod.Post, prescription);
                if (newPrescription == default)
                    return;
            }

            await DisplayAlert("", "Информация загружена", "Ок");
            await Navigation.PopAsync();
        }

        private List<Prescription> prescriptions = new List<Prescription>();

        private async void addPrescriptionButton_Clicked(object sender, EventArgs e)
        {
            if(prescriptionListView.ItemsSource != null && (prescriptionListView.ItemsSource as List<Prescription>).Count == 10)
            {
                await DisplayAlert("", "Рецептов не может быть более десяти.", "Ок");
                return;
            }

            string medicamentName = await DisplayPromptAsync("Добавление рецепта", "Введите название препората");
            if (medicamentName == null) return;
            string dose = await DisplayPromptAsync("Добавление рецепта", "Введите дозировку препората", keyboard: Keyboard.Numeric);
            if (dose == null) return;
            string format = await DisplayPromptAsync("Добавление рецепта", "Введите формат приема");
            if (format == null) return;

            if (medicamentName == "" || dose == "" || format == "")
            {
                await DisplayAlert("", "Одно из полей не заполнено, добавление отменено", "Ок");
                return;
            }

            Prescription prescription = new Prescription()
            {
                medicament = new Medicament()
                {
                    medicamentName = medicamentName
                },
                dose = Convert.ToDouble(dose),
                format = format
            };

            prescriptions.Add(prescription);

            prescriptionListView.ItemsSource = null;
            prescriptionListView.ItemsSource = prescriptions;
        }

        private void deletePrescriptionButton_Clicked(object sender, EventArgs e)
        {
            Prescription prescription = prescriptionListView.SelectedItem as Prescription;
            prescriptions.Remove(prescription);

            prescriptionListView.ItemsSource = null;
            prescriptionListView.ItemsSource = prescriptions;
        }

        MediaRecorder recorder;
        bool isRecording = false;

        async void RecordAudio()
        {
            await Permissions.RequestAsync<Permissions.Microphone>();

            try
            {
                if (isRecording)
                {
                    recorder.Stop();
                    isRecording = false;
                    isAudioMessageExsist = true;
                }
                else
                {
                    Java.IO.File path = new Java.IO.File(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "record.3gp");
                    path.ParentFile.Mkdirs();
                    if (recorder == null)
                    {
                        recorder = new MediaRecorder();
                    }

                    recorder.Reset();
                    recorder.SetAudioSource(AudioSource.Mic);
                    recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                    recorder.SetAudioEncoder(AudioEncoder.AmrNb);

                    recorder.SetOutputFile(path.AbsolutePath);
                    recorder.Prepare();
                    recorder.Start();
                    isRecording = true;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message, "Ок");
            }
        }

        private void RecordAudioButton_Clicked(object sender, EventArgs e)
        {
            RecordAudio();
        }
    }
}