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

    Console.WriteLine($"Request path: {path}");

    if (path == "/" || path == "/favicon.ico")//просто грузим страницу
    {
        
    }
    else if (Regex.IsMatch(path, @"/Client/\d{1,}") && request.Method == "GET")
    {
        await GetClient(request, response);
    }
    else if (Regex.IsMatch(path, @"/Images/\S{1,}"))
    {
        await response.SendFileAsync(Directory.GetCurrentDirectory() + @"\wwwroot\" + path);
    }
});

app.Run();

int GetIdFromPath(string path)
{
    return Convert.ToInt32(path.Split('/')[2]);
}

async Task ReturnError(HttpResponse response, Exception exception)
{
    response.StatusCode = 400;
    await response.WriteAsJsonAsync(new { error = exception.Message });
}

async Task GetClient(HttpRequest request, HttpResponse response)
{
    try
    {
        int id = GetIdFromPath((string)request.Path);
        Client? client = db.clients.Find(id);
        if (client == null)
            throw new Exception("Клиента с таким кодом не существует");
        await response.WriteAsJsonAsync(client);
    }
    catch (Exception ex)
    {
        await ReturnError(response, ex);
    }
}