using Android.Media;
using Android.Util;
using Java.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DataBaseClasses;
using System.Diagnostics;

namespace MobileApp
{
    
    public partial class MainPage : ContentPage
    {
        //MediaRecorder recorder;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void FindClient(object sender, EventArgs e)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://192.168.147.66:5120/getClient/" + clientIdEntry.Text);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");
                //await client.SendAsync(request);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    var json = await responseContent.ReadAsStringAsync();
                    Client client = new Client();
                    await DisplayAlert("", json, "Ok");
                    var a = JsonConvert.DeserializeObject<DataBaseClasses.Client>(json);
                    //Client client = JsonConvert.DeserializeObject<Client>(json);
                    //clientInfoStackLayout.BindingContext = client;
                    //DisplayAlert("", client.FullName, "Ok");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"HelpLink:\n{ex.HelpLink}\n\nSource:\n{ex.Source}\n\nStackTrace:\n{ex.StackTrace}\n\nMessage:\n{ex.Message}", "Ok");
            }
        }
        /*
        private void StartRecording()
        {
            recorder = new MediaRecorder();
            recorder.SetAudioSource(AudioSource.Default);
            recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            recorder.SetOutputFile("test.mp3");
            recorder.SetAudioEncoder(AudioEncoder.AmrNb);

            try
            {
                recorder.Prepare();
            }
            catch (IOException ioe)
            {
                Log.Error("", ioe.ToString());
            }

            recorder.Start();
        }

        void StopRecording()
        {
            if (recorder == null)
                return;
            recorder.Stop();
            recorder.Release();
            recorder = null;
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            StartRecording();
        }

        private void Button_Released(object sender, EventArgs e)
        {
            StopRecording();
        }*/
    }

    /*
    public class Client
    {
        public int client_id { get; set; }
        public string photoPath { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string lastName { get; set; }
        public string passportNumberAndSeries { get; set; }
        public string passportGetInfo { get; set; }
        public DateTime birthDate { get; set; }
        public virtual Gender gender { get; set; }
        public string workPlace { get; set; }
        public string address { get; set; }
        public string phoneNumder { get; set; }
        public string email { get; set; }
        public int medicalCardNumber { get; set; }
        public DateTime getMedicalCardDate { get; set; }
        public DateTime lastVisitDate { get; set; }
        public DateTime nextVisitDate { get; set; }
        public string insurancePolicyNumber { get; set; }
        public DateTime insurancePolicyEndDate { get; set; }
        public string insuranceCompany { get; set; }
        public string fullName { get { return $"{secondName} {firstName} {lastName}"; } }
    }

    public class Gender
    {
        public int gender_id { get; private set; }
        public string genderName { get; set; }
    }*/
}
