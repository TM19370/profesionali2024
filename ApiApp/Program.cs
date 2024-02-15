using System.Text.RegularExpressions;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    if (path == "/" || path == "/favicon.ico")//просто грузим страницу
    {
        
    }
    else if (Regex.IsMatch(path, @"/Client/\d{1,}") && request.Method == "GET")
    {
        Console.WriteLine(path);
        await GetClient(request, response);
    }
});

app.Run();

int GetIdFromPath(string path)
{
    return Convert.ToInt32(path.Split('/')[2]);
}

async Task GetClient(HttpRequest request, HttpResponse response)
{
    int id = GetIdFromPath((string)request.Path);
    Client client = db.clients.Find(id);
    await response.WriteAsJsonAsync(client);
    Console.WriteLine(client.FullName);
}