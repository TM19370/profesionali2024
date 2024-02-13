using System.Text.RegularExpressions;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;

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
    else if (Regex.IsMatch(path, @"/getClient/\d{1,}") && request.Method == "GET")
    {
        Gender gender = db.genders.FirstOrDefault();
        Client client = db.clients.First();
        await GetClient(request, response);
    }
});

app.Run();

int GetIdFromPath(string path)
{
    return Convert.ToInt32(path.Split('/')[3/*????????????????*/]);
}

async Task GetClient(HttpRequest request, HttpResponse response)
{
    int id = Convert.ToInt32(((string)request.Path).Split('/')[2]);
    Console.WriteLine(id);

    Client client = db.clients.Find(id);
    await response.WriteAsJsonAsync(client);
}