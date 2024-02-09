using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection;
using МИС__ГКБ_Большие_Кабаны_;
//using static МИС__ГКБ_Большие_Кабаны_.DBInteract;
using System.Drawing;
using Web;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Microsoft.Win32;
using System.Windows;
using Xceed.Words.NET;
using Xceed.Document.NET;
using System.Text.RegularExpressions;
using System;

Web.DataBaseContext db = new Web.DataBaseContext();
//DataBaseContext db = new DataBaseContext();

string organizationName = "ГКБ Большие Кабаны";
string imageFileName = "NULL";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    if (path == "/")//просто грузим страницу
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/html/registration.html");
    }
    else if (Regex.IsMatch(path, @"/api/hospitalization/\d{1,}/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}") && request.Method == "POST")
    {
        await SetHospitalizationDateTime(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/hospitalization/\d{1,}") && request.Method == "GET")
    {
        await GetHospitalizationInfo(request, response);
    }
    else if(path == "/api/hospitalization" && request.Method == "POST")
    {
        await CreateHospitalization(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/cancelHospitalization/\d{1,}/\w{1,}") && request.Method == "POST")
    {
        await CancelHospitalization(request, response);
    }
    else if (path == "/api/clients" && request.Method == "POST")
    {
        await CreateClient(request, response);
    }
    else if (path == "/api/image" && request.Method == "POST")
    {
        await CreateImage(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/QRCode/\d{1,}") && request.Method == "GET")
    {
        await CreateQRCode(request, response);
    }
    else if (path == "/api/DecodeQRCode" && request.Method == "POST")
    {
        await DecodeQRCode(request, response);
    }
    else if(Regex.IsMatch(path, @"/api/personalData/\d{1,}") && request.Method == "GET")
    {
        await PersonalData(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/medicalCareContract/\d{1,}") && request.Method == "GET")
    {
        await MedicalCareContract(request, response);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { error = "API не найдено" });
    }
});

app.Run();

int GetIdFromPath(string path)
{
    return Convert.ToInt32(path.Split('/')[3]);
}

void ReplaceText(DocX file, string searchText, string newText)
{
    try
    {
        StringReplaceTextOptions replaceOptions = new StringReplaceTextOptions();
        replaceOptions.SearchValue = searchText;
        replaceOptions.NewValue = newText;
        file.ReplaceText(replaceOptions);
    }
    catch (Exception ex) { }
    
}

async Task CancelHospitalization(HttpRequest request, HttpResponse response)
{
    try
    {
        string path = request.Path;
        int hospitalizationId = GetIdFromPath(path);
        Hospitalization hospitalization = db.hospitalizations.Find(hospitalizationId);
        hospitalization.hospitalizationCancelInfo = path.Split('/')[4];
        db.hospitalizations.Find(hospitalizationId).Edit(hospitalization);
        db.SaveChanges();
    }
    catch (Exception exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { error = exception.Message });
    }
}

async Task SetHospitalizationDateTime(HttpRequest request, HttpResponse response)
{
    try
    {
        int hospitalizationId = GetIdFromPath(request.Path);
        List<Hospitalization> hospitalizations = db.hospitalizations.Where(x => x.hospitalization_id == hospitalizationId).ToList();
        if (hospitalizations.Count == 0)
            throw new Exception("Госпитацизации с таким кодом не существует");
        Hospitalization hospitalization = hospitalizations.First();
        hospitalization.hospitalizationStartDate = Convert.ToDateTime(((string)request.Path).Split('/').Last());
        db.hospitalizations.Find(hospitalization.hospitalization_id).Edit(hospitalization);
        db.SaveChanges();
    }
    catch (Exception exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { error = exception.Message });
    }
}

async Task GetHospitalizationInfo(HttpRequest request, HttpResponse response)
{
    try
    {
        int hospitalizationId = GetIdFromPath(request.Path);
        List<Hospitalization> hospitalizations = db.hospitalizations.Where(x => x.hospitalization_id == hospitalizationId).ToList();
        if (hospitalizations.Count == 0)
            throw new Exception("Госпитацизации с таким кодом не существует");
        await response.WriteAsJsonAsync(hospitalizations.First());
    }
    catch (Exception exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { error = exception.Message });
    }
}

async Task MedicalCareContract(HttpRequest request, HttpResponse response)
{
    int clientId = GetIdFromPath(request.Path);
    Client client = db.clients.Find(clientId);
    string FIO = $"{client.secondName} {client.firstName} {client.lastName}";

    string fileName = $"{Directory.GetCurrentDirectory()}/бланк договора предоставления платных медицинских услуг.docx";
    var doc = DocX.Load(fileName);
    
    ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd.MM.yyyy"));
    ReplaceText(doc, "<ORG>", organizationName);
    ReplaceText(doc, "<position , full name>", "/////////////////");
    ReplaceText(doc, "<osnovanie>", "/////////////////");
    ReplaceText(doc, "<clientFIO>", FIO);
    ReplaceText(doc, "<license>", "/////////////////");
    ReplaceText(doc, "<licenseStartDate>", "/////////////////");
    ReplaceText(doc, "<licenseEndDate>", "/////////////////");
    ReplaceText(doc, "<licenseORGNameAddressPhoneNumber>", "/////////////////");
    ReplaceText(doc, "<licenseORGEndDate>", "/////////////////");
    ReplaceText(doc, "<services>", "/////////////////");
    ReplaceText(doc, "<waitingDays>", "/////////////////");
    ReplaceText(doc, "<position>", "/////////////////");
    ReplaceText(doc, "<ORGAddress>", "/////////////////");
    ReplaceText(doc, "<ORGEmail>", "/////////////////");
    ReplaceText(doc, "<OGRN>", "/////////////////");
    ReplaceText(doc, "<INN>", "/////////////////");
    ReplaceText(doc, "<positionGW>", "/////////////////");
    ReplaceText(doc, "<IO Fam>", "/////////////////");
    ReplaceText(doc, "<clientAddress>", client.address);
    ReplaceText(doc, "<clientOtherAddresses>", "/////////////////");
    ReplaceText(doc, "<clientPassport>", $"{client.passportNumberAndSeries} {client.passportGetInfo}");
    ReplaceText(doc, "<clientPhoneNumber>", client.phoneNumder);
    ReplaceText(doc, "<clientIO Fam>", $"{client.firstName[0]}{client.lastName[0]} {client.secondName}");

    string newFileName = $"Договор предоставления платных медицинских услуг {FIO} от {DateTime.Now.ToString("dd-M-yyyy")}.docx";
    doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/medicalCareContract.docx");

    await response.WriteAsJsonAsync(new { fileName = newFileName });
}

async Task PersonalData(HttpRequest request, HttpResponse response)
{
    int clientId = GetIdFromPath(request.Path);
    Client client = db.clients.Find(clientId);
    string FIO = $"{client.secondName} {client.firstName} {client.lastName}";

    string fileName = $"{Directory.GetCurrentDirectory()}/бланк согласия на обработку персональных данных.docx";
    var doc = DocX.Load(fileName);

    ReplaceText(doc, "<FIO>", FIO);
    ReplaceText(doc, "<passportNumberAndSeries>", client.passportNumberAndSeries);
    ReplaceText(doc, "<passportGetInfo>", client.passportGetInfo);
    ReplaceText(doc, "<address>", client.address);
    ReplaceText(doc, "<ORG>", organizationName);
    ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd MMMM yyyyг."));
    ReplaceText(doc, "<target>", "медицинского обслуживания");

    string newFileName = $"Согласие на обработку персоняльных данных {FIO} от {DateTime.Now.ToString("dd-M-yyyy")}.docx";
    doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/personalData.docx");

    await response.WriteAsJsonAsync(new { fileName = newFileName });
}

async Task DecodeQRCode(HttpRequest request, HttpResponse response)
{
    IFormFile file = request.Form.Files[0];
    string filePath = $"{Directory.GetCurrentDirectory()}/QRToDecode.png";
    using (var fileStream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(fileStream);
    }

    string client_id;

    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
    {
        Bitmap img = new Bitmap(fileStream);
        QRCodeDecoder decoder = new QRCodeDecoder();
        client_id = decoder.Decode(new QRCodeBitmapImage(img));
    }

    Client client = db.clients.Find(Convert.ToInt32(client_id));
    await response.WriteAsJsonAsync(client);
}

async Task CreateQRCode(HttpRequest request, HttpResponse response)
{
    string qrCodeText = GetIdFromPath(request.Path).ToString();

    QRCodeEncoder encoder = new QRCodeEncoder();
    Bitmap qrCode = encoder.Encode(qrCodeText);
    string qrpath = $"{Directory.GetCurrentDirectory()}/wwwroot/qr.png";
    using (var fileStream = new FileStream(qrpath, FileMode.Create))
    {
        qrCode.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
    }

    await response.WriteAsync("QR код создан");
    //await response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory() + "/qr.png"));
}

async Task CreateImage(HttpRequest request, HttpResponse response)
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

async Task CreateHospitalization(HttpRequest request, HttpResponse response)
{
    try
    {
        Hospitalization? hospitalization = await request.ReadFromJsonAsync<Hospitalization>();
        if(hospitalization == null)
            throw new Exception("Некоректные данные");

        string passportNumberAndSeries = hospitalization.client.passportNumberAndSeries;

        List<Client> clients = db.clients.Where(x => x.passportNumberAndSeries == passportNumberAndSeries).ToList();
        Client client;
        if (clients.Count == 0)
        {
            client = hospitalization.client;
            client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
            db.clients.Add(client);
            db.SaveChanges();
            hospitalization.client = client;
        }
        else
        {
            hospitalization.client = db.clients.Find(clients.First().client_id);
        }

        db.hospitalizations.Add(hospitalization);
        db.SaveChanges();
    }
    catch (Exception exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { error = exception.Message });
    }
}

async Task CreateClient(HttpRequest request, HttpResponse response)
{
    try
    {
        if (imageFileName == "NULL")
        {
            throw new Exception("Произошла ошибка при загрузке изображения");
        }

        Client? client = await request.ReadFromJsonAsync<Client>();
        if (client == null)
            throw new Exception("Некоректные данные");
        client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
        client.photoPath = imageFileName;

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
    catch (Exception exception)
    {
        string fullPath = $"{Directory.GetCurrentDirectory()}/clientImages/{imageFileName}";
        File.Delete(fullPath);
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { error = exception.Message });
    }
    imageFileName = "NULL";
}