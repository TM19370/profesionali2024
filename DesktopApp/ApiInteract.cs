using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopApp
{
    public class ApiInteract
    {
        private static HttpClient _httpClient = new HttpClient();
        public static async Task<T> SendRequest<T>(string url, HttpMethod httpMethod, T content)
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
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            HttpContent responseContent = response.Content;
            string json = await responseContent.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ErrorResponce errorResponce = JsonConvert.DeserializeObject<ErrorResponce>(json);
                ShowError(errorResponce.error, "");
                return default;
            }

            T responseObject = JsonConvert.DeserializeObject<T>(json);
            return responseObject;
        }

        public static void ShowError(string text, string header)
        {
            MessageBox.Show(text, header, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public class ErrorResponce
        {
            public string error { get; set; }
        }
    }
}
