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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
                request.RequestUri = new Uri("http://192.168.147.66:5120/Client/" + clientIdEntry.Text);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");
                //await client.SendAsync(request);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    var json = await responseContent.ReadAsStringAsync();
                    await DisplayAlert("", json, "Ok");
                    Client client = JsonConvert.DeserializeObject<Client>(json);
                    clientInfoStackLayout.BindingContext = client;
                    //DisplayAlert("", client.FullName, "Ok");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"HelpLink:\n{ex.HelpLink}\n\nSource:\n{ex.Source}\n\nStackTrace:\n{ex.StackTrace}\n\nMessage:\n{ex.Message}", "Ok");
            }
        }


        protected MediaRecorder recorder;


        private void StartRecording(string filePath)
        {
            try
            {
                if(recorder == null)
                {
                    recorder = new MediaRecorder();
                }
                else
                {
                    recorder.Reset();
                    recorder.SetAudioSource(AudioSource.Mic);
                    recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                    recorder.SetAudioEncoder(AudioEncoder.AmrNb);

                    recorder.SetOutputFile(filePath);
                    recorder.Prepare();
                    recorder.Start();
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message, "Ok");
            }
            /*
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

            recorder.Start();*/
        }

        void StopRecording()
        {/*
            if (recorder == null)
                return;
            recorder.Stop();
            recorder.Release();
            recorder = null;*/
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            /*if (ActivityCompat.checkSelfPermission(activity(), Manifest.permission.RECORD_AUDIO) != PackageManager.PERMISSION_GRANTED)
            {
                ActivityCompat.requestPermissions(activity(), new String[] { Manifest.permission.RECORD_AUDIO }, BuildDev.RECORD_AUDIO);
            }
            else
            {
                startRecording();
            }*/
            StartRecording("test.mp3");
        }

        private void Button_Released(object sender, EventArgs e)
        {
            StopRecording();
        }
    }

    
    public class Client
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int client_id { get; set; }
        public string photoPath { get; set; }
        [Required] public string firstName { get; set; }
        [Required] public string secondName { get; set; }
        [Required] public string lastName { get; set; }
        [Required] public string passportNumberAndSeries { get; set; }
        [Required] public string passportGetInfo { get; set; }
        [Required] public DateTime birthDate { get; set; }
        [Required] public virtual Gender gender { get; set; }
        [Required] public string workPlace { get; set; }
        [Required] public string address { get; set; }
        [Required] public string phoneNumder { get; set; }
        [Required] public string email { get; set; }
        [Required] public int medicalCardNumber { get; set; }
        [Required] public DateTime getMedicalCardDate { get; set; }
        [Required] public DateTime lastVisitDate { get; set; }
        [Required] public DateTime nextVisitDate { get; set; }
        [Required] public string insurancePolicyNumber { get; set; }
        [Required] public DateTime insurancePolicyEndDate { get; set; }
        [Required] public string insuranceCompany { get; set; }
        public string FullName { get { return $"{secondName} {firstName} {lastName}"; } }
    }
    
    public class Gender
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int gender_id { get; private set; }
        [Required] public string genderName { get; set; }
    }
}
