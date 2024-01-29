using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection;
using МИС__ГКБ_Большие_Кабаны_;
using static МИС__ГКБ_Большие_Кабаны_.DBInteract;

string imageFileName = "NULL";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    if (path == "/api/clients" && request.Method == "POST")
    {
        await CreateClient(request, response);
    }
    else if (path == "/api/image" && request.Method == "POST")
    {
        IFormFile file = request.Form.Files[0];
        var uploadPath = $"{Directory.GetCurrentDirectory()}/clientImages";
        Directory.CreateDirectory(uploadPath);

        imageFileName = $"{DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss-FFF")}.{file.FileName.Split('.').Last()}";
        string fullPath = $"{uploadPath}/{imageFileName}";

        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        await response.WriteAsync("Изображение загруженно");
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/html/registration.html");
    }
});

app.Run();

async Task CreateClient(HttpRequest request, HttpResponse response)
{
    try
    {
        if (imageFileName == "NULL")
        {
            throw new Exception("Произошла ошибка при загрузке изображения");
        }

        ClientPost clientPost = await request.ReadFromJsonAsync<ClientPost>();
        Gender gender = db.genders.Find(clientPost.gender);
        Client client = new Client()
        {
            photoPath = imageFileName,
            firstName = clientPost.firstName,
            secondName = clientPost.secondName,
            lastName = clientPost.lastName,
            passportNumberAndSeries = clientPost.passportNumberAndSeries,
            birthDate = clientPost.birthDate,
            gender = gender,
            address = clientPost.address,
            phoneNumder = clientPost.phoneNumder,
            email = clientPost.email,
            medicalCardNumber = clientPost.medicalCardNumber,
            getMedicalCardDate = clientPost.getMedicalCardDate,
            lastVisitDate = clientPost.lastVisitDate,
            nextVisitDate = clientPost.nextVisitDate,
            insurancePolicyNumber = clientPost.insurancePolicyNumber,
            insurancePolicyEndDate = clientPost.insurancePolicyEndDate
        };
        if (client != null)
        {
            List<Client> clients = db.clients.Where(x => x.passportNumberAndSeries == client.passportNumberAndSeries).ToList();
            if (clients.Count != 0)
                throw new Exception("Пользователь с таким пасспортом уже существует");
            clients = db.clients.Where(x => x.medicalCardNumber == client.medicalCardNumber).ToList();
            if (clients.Count != 0)
                throw new Exception("Пользователь с таким идентификационным кодом медицинской карты уже существует");
            db.clients.Add(client);
            db.SaveChanges();

            await response.WriteAsync("Данные успешно загружены");
        }
        else
        {
            throw new Exception("Некоректные данные");
        }
    }
    catch (Exception exception)
    {
        string fullPath = $"{Directory.GetCurrentDirectory()}/clientImages/{imageFileName}";
        File.Delete(fullPath);
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { error = exception.Message });
    }
    imageFileName = "NULL";
}

public class ClientPost
{
    public string firstName { get; set; }
    public string secondName { get; set; }
    public string lastName { get; set; }
    public string passportNumberAndSeries { get; set; }
    public DateTime birthDate { get; set; }
    public string gender { get; set; }
    public string address { get; set; }
    public string phoneNumder { get; set; }
    public string email { get; set; }
    public int medicalCardNumber { get; set; }
    public DateTime getMedicalCardDate { get; set; }
    public DateTime lastVisitDate { get; set; }
    public DateTime nextVisitDate { get; set; }
    public int insurancePolicyNumber { get; set; }
    public DateTime insurancePolicyEndDate { get; set; }
}