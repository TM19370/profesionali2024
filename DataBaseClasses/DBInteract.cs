using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseClasses
{
    public class DBInteract
    {
        private static string apiServerUrl = "http://192.168.147.66:5120";
        private static HttpClient httpClient = new HttpClient();

        public static DataBaseContext db = new DataBaseContext();

        public static async Task<Client?> GetClientByID(int client_id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(apiServerUrl + "/Client/" + client_id);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            Client? client;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var json = await responseContent.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(json);
            }
            else
            {
                client = null;
            }
            return client;
        }

        private static async Task<HttpResponseMessage> SendRequest(HttpRequestMessage httpRequestMessage)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await httpClient.SendAsync(httpRequestMessage);
            }
            catch (Exception ex)
            {

            }
            
            return response;
        }
    }
}
