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
//using DataBaseClasses;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Android.Security;
using Android.Content.Res;

namespace MobileApp
{
    
    public partial class EMCPage : ContentPage
    {
        private string apiServerPath = "http://192.168.147.66:5120";

        public EMCPage()
        {
            InitializeComponent();
        }

        private async void FindClient(object sender, EventArgs e)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(apiServerPath + "/clients/" + clientIdEntry.Text);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                HttpContent responseContent = response.Content;
                var json = await responseContent.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Client client = JsonConvert.DeserializeObject<Client>(json);
                    clientInfoStackLayout.BindingContext = client;
                    clientImage.Source = apiServerPath + "/Images/" + client.photoPath;
                }
                else
                {
                    ErrorResponce errorResponce = JsonConvert.DeserializeObject<ErrorResponce>(json);
                    throw new Exception(errorResponce.error);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"{ex.Message}", "Ok");
                //DisplayAlert("Ошибка", $"HelpLink:\n{ex.HelpLink}\n\nSource:\n{ex.Source}\n\nStackTrace:\n{ex.StackTrace}\n\nMessage:\n{ex.Message}", "Ok");
            }
        }
    }

    
    public class ErrorResponce
    {
        public string error { get; set; }
    }
}
