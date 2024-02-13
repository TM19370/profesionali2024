using System.Text.RegularExpressions;
using МИС__ГКБ_Большие_Кабаны_;
//using System.Configuration.Assemblies;
//using System.Data.Entity;
using ApiApp;

ApiApp.DataBaseContext db = new ApiApp.DataBaseContext();
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    try
    {
        if (Regex.IsMatch(path, @"/getClient/\d{1,}") && request.Method == "GET")
        {
            int id = Convert.ToInt32(((string)path).Split('/')[2]);
            Console.WriteLine(id);

            Client client = db.clients.Find(id);
            await response.WriteAsJsonAsync(client);
        }
    }
    catch (Exception ex) { }
});

app.Run();
