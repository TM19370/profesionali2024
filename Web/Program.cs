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

    /*try
    {
        Gender gender = "2";
       // string gender = "2";
        Client client1 = new Client()
        {
            firstName = "test",
            secondName = "test",
            nextVisitDate = DateTime.Now,
            lastName = "test",
            lastVisitDate = DateTime.Now,
            address = "test",
            birthDate = DateTime.Now,
            getMedicalCardDate = DateTime.Now,
            passportNumberAndSeries = "test",
            email = "test",
            gender = gender,
            insurancePolicyEndDate = DateTime.Now,
            insurancePolicyNumber = "test",
            medicalCardNumber = 1,
            passportGetInfo = "test",
            photoPath = "test",
            phoneNumder = "test"
        };
        db.clients.Add(client1);
        db.SaveChanges();
    }
    catch (Exception ex) { }*/

    ///////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////             Сделать запросы по типу     path/ID
    ///////////////////////////////////////////////////////////

    if (path == "/")//просто грузим страницу
    {
        Client client = db.clients.Where(x => x.client_id == 333).First();
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/html/registration.html");
    }
    else if (Regex.IsMatch(path, @"/api/hospitalization/\d{1,}") && request.Method == "GET")
    {
        await GetHospitalizationInfo(request, response);
    }
    else if(path == "/api/hospitalization" && request.Method == "POST")
    {
        await CreateHospitalization(request, response);
    }
    else if (path == "/api/clients" && request.Method == "POST")
    {
        await CreateClient(request, response);
    }
    else if (path == "/api/image" && request.Method == "POST")
    {
        await CreateImage(request, response);
    }
    else if (path == "/api/QRCode" && request.Method == "POST")
    {
        await CreateQRCode(request, response);
    }
    else if (path == "/api/DecodeQRCode" && request.Method == "POST")
    {
        await DecodeQRCode(request, response);
    }
    else if(path == "/api/personalData" && request.Method == "POST")
    {
        await PersonalData(request, response);
    }
    else if (path == "/api/medicalCareContract" && request.Method == "POST")
    {
        await MedicalCareContract(request, response);
    }/*
    else
    {
        Client client = db.clients.Where(x => x.client_id == 333).First();
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/html/registration.html");
    }*/
});

app.Run();

int GetIdFromPath(string path)
{
    return Convert.ToInt32(path.Split('/').Last());
}

async Task GetHospitalizationInfo(HttpRequest request, HttpResponse response)
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}

async Task MedicalCareContract(HttpRequest request, HttpResponse response)
{
    var clientIdClass = await request.ReadFromJsonAsync<ClientIdClass>();
    int clientId = Convert.ToInt32(clientIdClass!.id);

    Client client = db.clients.Find(clientId);

    string fileName = $"{Directory.GetCurrentDirectory()}/бланк договора предоставления платных медицинских услуг.docx";

    string FIO = $"{client.secondName} {client.firstName} {client.lastName}";

    var doc = DocX.Load(fileName);

    StringReplaceTextOptions options = new StringReplaceTextOptions();
    
    doc.ReplaceText("<currentDate>", DateTime.Now.ToString("dd.MM.yyyy"));
    doc.ReplaceText("<ORG>", organizationName);
    doc.ReplaceText("<position , full name>", "/////////////////");
    doc.ReplaceText("<osnovanie>", "/////////////////");
    doc.ReplaceText("<clientFIO>", FIO);
    doc.ReplaceText("<license>", "/////////////////");
    doc.ReplaceText("<licenseStartDate>", "/////////////////");
    doc.ReplaceText("<licenseEndDate>", "/////////////////");
    doc.ReplaceText("<licenseORGNameAddressPhoneNumber>", "/////////////////");
    doc.ReplaceText("<licenseORGEndDate>", "/////////////////");
    doc.ReplaceText("<services>", "/////////////////");
    doc.ReplaceText("<waitingDays>", "/////////////////");
    doc.ReplaceText("<position>", "/////////////////");
    doc.ReplaceText("<ORGAddress>", "/////////////////");
    doc.ReplaceText("<ORGEmail>", "/////////////////");
    doc.ReplaceText("<OGRN>", "/////////////////");
    doc.ReplaceText("<INN>", "/////////////////");
    doc.ReplaceText("<positionGW>", "/////////////////");
    doc.ReplaceText("<IO Fam", "/////////////////");
    doc.ReplaceText("<clientAddress>", client.address);
    doc.ReplaceText("<clientOtherAddresses>", "/////////////////");
    doc.ReplaceText("<clientPassport>", $"{client.passportNumberAndSeries} {client.passportGetInfo}");
    doc.ReplaceText("<clientPhoneNumber>", client.phoneNumder);
    doc.ReplaceText("<clientIO Fam>", $"{client.firstName[0]}{client.lastName[0]} {client.secondName}");

    string newFileName = $"Договор предоставления платных медицинских услуг {FIO} от {DateTime.Now.ToString("dd-M-yyyy")}.docx";

    doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/medicalCareContract.docx");

    await response.WriteAsJsonAsync(new { fileName = newFileName });
}

async Task PersonalData(HttpRequest request, HttpResponse response)
{
    var clientIdClass = await request.ReadFromJsonAsync<ClientIdClass>();
    int clientId = Convert.ToInt32(clientIdClass!.id);

    Client client = db.clients.Find(clientId);

    string fileName = $"{Directory.GetCurrentDirectory()}/бланк согласия на обработку персональных данных.docx";

    string FIO = $"{client.secondName} {client.firstName} {client.lastName}";

    var doc = DocX.Load(fileName);

    StringReplaceTextOptions options = new StringReplaceTextOptions();
    //options.SearchValue = "<FIO>";
    //options.NewValue = FIO;
    //doc.ReplaceText(options);
    doc.ReplaceText("<FIO>", FIO);
    doc.ReplaceText("<passportNumberAndSeries>", client.passportNumberAndSeries);
    doc.ReplaceText("<passportGetInfo>", client.passportGetInfo);////////////////////////////////////////////////////////////
    doc.ReplaceText("<address>", client.address);
    doc.ReplaceText("<ORG>", organizationName);
    doc.ReplaceText("<currentDate>", DateTime.Now.ToString("dd MMMM yyyyг."));
    doc.ReplaceText("<target>", "медицинского обслуживания");

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
    ClientPost clientPost = new ClientPost()
    {
        firstName = client.firstName,
        secondName = client.secondName,
        lastName = client.lastName,
        passportNumberAndSeries = client.passportNumberAndSeries,
        birthDate = client.birthDate,
        gender = client.gender.genderName,
        address = client.address,
        phoneNumder = client.phoneNumder,
        email = client.email,
        medicalCardNumber = client.medicalCardNumber,
        getMedicalCardDate = client.getMedicalCardDate,
        lastVisitDate = client.lastVisitDate,
        nextVisitDate = client.nextVisitDate,
        insurancePolicyNumber = client.insurancePolicyNumber,
        insurancePolicyEndDate = client.insurancePolicyEndDate
    };
    await response.WriteAsJsonAsync(clientPost);
}

async Task CreateQRCode(HttpRequest request, HttpResponse response)
{
    var qrCodeText = await request.ReadFromJsonAsync<ClientIdClass>();

    QRCodeEncoder encoder = new QRCodeEncoder();
    Bitmap qrCode = encoder.Encode(qrCodeText!.id);
    string qrpath = $"{Directory.GetCurrentDirectory()}/wwwroot/qr.png";
    using (var fileStream = new FileStream(qrpath, FileMode.Create))
    {
        qrCode.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
    }

    await response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(), "qr.png"));
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
        //Client client = await request.ReadFromJsonAsync<Client>();
        ClientPost? clientPost = await request.ReadFromJsonAsync<ClientPost>();
        if (clientPost == null)
            return;

        Gender gender = db.genders.Find(clientPost.gender);
        Hospitalization hospitalization = new Hospitalization()
        {
            firstName = clientPost.firstName,
            secondName = clientPost.secondName,
            lastName = clientPost.lastName,
            passportNumberAndSeries = clientPost.passportNumberAndSeries,
            birthDate = clientPost.birthDate,
            gender = gender,
            workPlace = clientPost.workPlace,
            address = clientPost.address,
            phoneNumder = clientPost.phoneNumder,
            email = clientPost.email,
            medicalCardNumber = clientPost.medicalCardNumber,
            getMedicalCardDate = clientPost.getMedicalCardDate,
            lastVisitDate = clientPost.lastVisitDate,
            nextVisitDate = clientPost.nextVisitDate,
            insurancePolicyNumber = clientPost.insurancePolicyNumber,
            insurancePolicyEndDate = clientPost.insurancePolicyEndDate,
            hospitalizationStartDate = DateTime.Now,
            hospitalizationEndDate = DateTime.Now
        };
        db.hospitalizations.Add(hospitalization);
        db.SaveChanges();
        if(hospitalization == null)
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
}

async Task CreateClient(HttpRequest request, HttpResponse response)
{
    try
    {
        if (imageFileName == "NULL")
        {
            throw new Exception("Произошла ошибка при загрузке изображения");
        }

        ClientPost? clientPost = await request.ReadFromJsonAsync<ClientPost>();
        if (clientPost == null)
            return;

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



/// <summary>
/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
public class ClientIdClass
{
    public string? id { get; set;}
}

public class ClientPost
{
    public string? firstName { get; set; }
    public string? secondName { get; set; }
    public string? lastName { get; set; }
    public string? passportNumberAndSeries { get; set; }
    public DateTime birthDate { get; set; }
    public string? gender { get; set; }
    public string? workPlace { get; set; }
    public string? address { get; set; }
    public string? phoneNumder { get; set; }
    public string? email { get; set; }
    public int medicalCardNumber { get; set; }
    public DateTime getMedicalCardDate { get; set; }
    public DateTime lastVisitDate { get; set; }
    public DateTime nextVisitDate { get; set; }
    public string? insurancePolicyNumber { get; set; }
    public DateTime insurancePolicyEndDate { get; set; }
}