//using DataBaseClasses;
using Android.Media;
using Java.IO;
//using DataBaseClasses.Migrations;
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
            /*
            List<Prescription> prescriptions = new List<Prescription>();

            for (int i = 0; i < 10; i++)
            {
                prescriptions.Add(new Prescription()
                {
                    medicament = new Medicament()
                    {
                        medicamentName = "Пропитал"
                    },
                    dose = 0.5,
                    format = "qqqqq"
                });
            }

            prescriptionListView.ItemsSource = prescriptions;

            List<Prescription> prescriptions1 = new List<Prescription>();
            prescriptions1 = prescriptionListView.ItemsSource as List<Prescription>;*/
        }

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

        private static HttpClient httpClient = new HttpClient();

        private async void addAppointmentInfoButton_Clicked(object sender, EventArgs e)
        {
            Client client = await SendRequest<Client>("/Client/" + clientIdEntry.Text, HttpMethod.Get, null);
            if (client == default)
                return;

            AppointmentInfo appointmentInfo = new AppointmentInfo()
            {
                client = client,
                anamnesis = anamnesisEditor.Text,
                symptoms = symptomsEditor.Text,
                diagnosis = diagnosisEditor.Text,
                recommendations = recommendationsEditor.Text
            };
            appointmentInfo = await SendRequest<AppointmentInfo>("/AppointmentInfo", HttpMethod.Post, appointmentInfo);
            if (appointmentInfo == default)
                return;

            List<Prescription> prescriptions = new List<Prescription>();
            prescriptions = prescriptionListView.ItemsSource as List<Prescription>;

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

            await DisplayAlert("", "Информация загружена", "Ok");
            await Navigation.PopAsync();
        }

        private List<Prescription> prescriptions = new List<Prescription>();

        private async void addPrescriptionButton_Clicked(object sender, EventArgs e)
        {
            string medicamentName = await DisplayPromptAsync("Добавление рецепта", "Введите название препората");
            string dose = await DisplayPromptAsync("Добавление рецепта", "Введите дозировку препората", keyboard: Keyboard.Numeric);
            string format = await DisplayPromptAsync("Добавление рецепта", "Введите формат приема");

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
        bool b = false;

        async void RecordAudio()
        {
            await Permissions.RequestAsync<Permissions.Microphone>();

            try
            {
                if (b)
                {
                    recorder.Stop();
                }
                else
                {
                    File path = new File(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "test.3gp/testmedia.3gp")/*System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic) + "/tst.3gp"*/;
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

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message, "Ok");
            }
            b = !b;
            /*byte[] audioBuffer = new byte[100000];
            var audRecorder = new AudioRecord(
              // Hardware source of recording.
              AudioSource.Mic,
              // Frequency
              11025,
              // Mono or stereo
              ChannelIn.Mono,
              // Audio encoding
              Android.Media.Encoding.Pcm16bit,
              // Length of the audio clip.
              audioBuffer.Length
            );

            audRecorder.StartRecording();
            while (true)
            {
                try
                {
                    // Keep reading the buffer while there is audio input.
                    audRecorder.Read(audioBuffer, 0, audioBuffer.Length);
                    // Write out the audio file.
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.Message);
                    break;
                }
            }*/
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            RecordAudio();
        }
    }
}