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

Web.DataBaseContext db = new Web.DataBaseContext();
//DataBaseContext db = new DataBaseContext();

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
    else if (path == "/api/QRCode" && request.Method == "POST")
    {
        var qrCodeText = await request.ReadFromJsonAsync<ClientIdClass>();

        QRCodeEncoder encoder = new QRCodeEncoder();
        Bitmap qrCode = encoder.Encode(qrCodeText.id);
        string qrpath = $"{Directory.GetCurrentDirectory()}/wwwroot/qr.png";
        using (var fileStream = new FileStream(qrpath, FileMode.Create))
        {
            qrCode.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
        }

        await response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(), "qr.png"));
    }
    else if (path == "/api/DecodeQRCode" && request.Method == "POST")
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
        ClientPost clientPost = new ClientPost() { 
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
    else if(path == "/api/personalData" && request.Method == "POST")
    {
        var clientIdClass = await request.ReadFromJsonAsync<ClientIdClass>();
        int clientId = Convert.ToInt32(clientIdClass.id);

        Client client = db.clients.Find(clientId);

        string fileName = $"{Directory.GetCurrentDirectory()}/бланк согласия на обработку персональных данных.docx";

        string FIO = $"{client.secondName} {client.firstName} {client.lastName}";

        var doc = DocX.Load(fileName);

        doc.ReplaceText("<FIO>", FIO);
        doc.ReplaceText("<passportNumberAndSeries>", client.passportNumberAndSeries);
        doc.ReplaceText("<passportGetInfo>", "/////////////////////////////");////////////////////////////////////////////////////////////
        doc.ReplaceText("<address>", client.address);
        doc.ReplaceText("<ORG>", "ГКБ Большие Кабаны");
        doc.ReplaceText("<currentDate>", DateTime.Now.ToString("dd MMMM yyyyг."));
        doc.ReplaceText("<target>", "медицинского обслуживания");

        string newFileName = $"Согласие на обработку персоняльных данных {FIO} от {DateTime.Now.ToString("dd-M-yyyy")}.docx";

        doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/personalData.docx");

        await response.WriteAsJsonAsync(new { fileName = newFileName });
    }
    else
    {
        Client client = db.clients.Where(x => x.client_id == 333).First();
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/html/registration.html");
    }
});

app.Run();

void v()
{
    string qrCodeText = "333";

    QRCodeEncoder encoder = new QRCodeEncoder();
    Bitmap qrCode = encoder.Encode(qrCodeText);
    qrCode.Save("qr.png");

    using (FileStream fileStream = new FileStream($"{Directory.GetCurrentDirectory()}/qr.png", FileMode.Open))
    {
        Bitmap img = new Bitmap(fileStream);
        QRCodeDecoder decoder = new QRCodeDecoder();
        string mesage = decoder.Decode(new QRCodeBitmapImage(img));
    }
}

void word()
{
    string fileName = $"{Directory.GetCurrentDirectory()}/бланк согласия на обработку персональных данных.docx";

    var doc = DocX.Load(fileName);

    doc.ReplaceText("<FIO>", "Иванов Иван Иванович");
    doc.ReplaceText("<passportNumberAndSeries>", "2222222222");
    doc.ReplaceText("<passportGetInfo>", "11.01.2005 г Оренбург");
    doc.ReplaceText("<address>", "г. Оренбург ул. Терешковой д. 2 кв. 12");
    doc.ReplaceText("<ORG>", "ГКБ Большие Кабаны");
    doc.ReplaceText("<currentDate>", DateTime.Now.ToString("dd MMMM yyyyг."));
    doc.ReplaceText("<target>", "Медицинского обслуживания");

    doc.SaveAs($"{Directory.GetCurrentDirectory()}/test.docx");
}

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

public class ClientIdClass
{
    public string id { get; set;}
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
    public string insurancePolicyNumber { get; set; }
    public DateTime insurancePolicyEndDate { get; set; }
}