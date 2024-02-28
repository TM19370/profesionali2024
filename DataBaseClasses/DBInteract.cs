using DataBaseClasses.Migrations;
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
        public static string apiServerUrl = "http://192.168.147.66:5120";
        public static DataBaseContext db = new DataBaseContext();

        public class ErrorResponce
        {
            public string error { get; set; }
        }
    }
}
