using System.Text.RegularExpressions;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using Newtonsoft.Json;
using System.Drawing;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Xceed.Words.NET;
using Xceed.Document.NET;

string organizationName = "��� ������� ������";
string imageFileName = "";
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5079",
                                              "http://192.168.147.66:5120/") // ����������� ������
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();  
                      });
});


var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);


Random random = new Random();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.Write($"{DateTime.Now.ToString("HH:mm:ss:fff")} Request path: ");
    Console.ResetColor();
    Console.Write(path);
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.Write("    Request method: ");
    Console.ResetColor();
    Console.WriteLine(request.Method);

    if ((path == "/") || (path == "/favicon.ico"))//������ ������ ��������
    {
        
    }
    else if (Regex.IsMatch(path, @"/Client/\d{1,}") && request.Method == "GET")
    {
        //��������� �������
        await GetClient(request, response);
    }
    else if (Regex.IsMatch(path, @"/Images/\S{1,}"))
    {
        //���������� �����������
        await response.SendFileAsync(Directory.GetCurrentDirectory() + @"\wwwroot\" + path);
    }
    else if(path == "/Medicament" && request.Method == "POST")
    {
        await FindOrCreateMedicament(request, response);
    }
    else if(path == @"/AppointmentInfo" && request.Method == "POST")
    {
        await CreateAppointmentInfo(request, response);
    }
    else if(path == @"/Prescription" && request.Method == "POST")
    {
        await CreatePrescription(request, response);
    }
    else if (path == "/Audio" && request.Method == "POST")
    {
        await SaveAudioMessage(request, response);
    }
    else if (Regex.IsMatch(path, @"/Hospitalization/\d{1,}/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}") && request.Method == "POST")
    {
        // ��������� ���� � ������� ��������������
        await SetHospitalizationDateTime(request, response);
    }
    else if (Regex.IsMatch(path, @"/Hospitalization/\d{1,}") && request.Method == "GET")
    {
        // ��������� ���������� � ��������������
        await GetHospitalizationInfo(request, response);
    }
    else if (path == "/Hospitalization" && request.Method == "POST")
    {
        // �������� ��������������
        await CreateHospitalization(request, response);
    }
    else if (Regex.IsMatch(path, @"/CancelHospitalization/\d{1,}/\w{1,}") && request.Method == "POST")
    {
        // ������ ��������������
        await CancelHospitalization(request, response);
    }
    else if (path == "/Clients" && request.Method == "POST")
    {
        // �������� �������
        await CreateClient(request, response);
    }
    else if (path == "/Image" && request.Method == "POST")
    {
        // �������� �����������
        await CreateImage(request, response);
    }
    else if (Regex.IsMatch(path, @"/QRCode/\d{1,}") && request.Method == "GET")
    {
        // �������� QR ����
        await CreateQRCode(request, response);
    }
    else if (path == "/DecodeQRCode" && request.Method == "POST")
    {
        // ����������� QR ����
        await DecodeQRCode(request, response);
    }
    else if (Regex.IsMatch(path, @"/PersonalData/\d{1,}") && request.Method == "GET")
    {
        // ���������� �������� �� ��������� ������������ �������
        await PersonalData(request, response);
    }
    else if (Regex.IsMatch(path, @"/MedicalCareContract/\d{1,}") && request.Method == "GET")
    {
        // ���������� �������� �������������� ������� ����������� �����
        await MedicalCareContract(request, response);
    }
    else
    {
        // Api � ����� ������� �� ����������
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { error = "API �� �������" });
    }

    Console.WriteLine($"Response status code: {response.StatusCode}");
});

app.Run();

int GetIdFromPath(string path)
{
    return Convert.ToInt32(path.Split('/')[2]);
}

string GetRequestData(string path)
{
    return path.Split("/")[3];
}

async Task ReturnError(HttpResponse response, Exception exception)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write("Error: ");
    Console.ResetColor();
    Console.WriteLine(exception.Message);
    response.StatusCode = 400;
    await response.WriteAsJsonAsync(new { error = exception.Message });
}

async Task SaveAudioMessage(HttpRequest request, HttpResponse response)
{
    // ������� �������� ����������� ������
    IFormFile file = request.Form.Files.First();
    // ���� � �����, ��� ����� ��������� �����
    var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
    // ������� ����� ��� �������� ������
    Directory.CreateDirectory(uploadPath);

    // ��������� ���� � ����� � ����� uploads
    string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss-FFF")}{random.Next(0, 1000).ToString("G3")}.{file.FileName.Split('.').Last()}";
    string fullPath = $"{uploadPath}/{fileName}";

    // ��������� ���� � ����� uploads
    using (var fileStream = new FileStream(fullPath, FileMode.Create))
    {
        await file.CopyToAsync(fileStream);
    }

    await response.WriteAsync(fileName);
}

async Task FindOrCreateMedicament(HttpRequest request, HttpResponse response)
{
    try
    {
        Medicament? medicament = await request.ReadFromJsonAsync<Medicament>();
        if (medicament == null)
            throw new Exception("�������� ����������� �����");
        List<Medicament> medicaments = db.Medicaments.Where(x => x.medicamentName == medicament.medicamentName).ToList();

        if(medicaments.Count == 0)
        {
            db.Medicaments.Add(medicament);
            db.SaveChanges();
        }

        medicament = db.Medicaments.Where(x => x.medicamentName == medicament.medicamentName).First();

        await response.WriteAsJsonAsync(medicament);
    }
    catch (Exception ex)
    {
        await ReturnError(response, ex);
    }
}

async Task CreateAppointmentInfo(HttpRequest request, HttpResponse response)
{
    try
    {
        AppointmentInfo? appointmentInfo = await request.ReadFromJsonAsync<AppointmentInfo>();
        if (appointmentInfo == null)
            throw new Exception("���������� � ������ �����");

        int clientId = appointmentInfo.client.client_id;
        Client? client = db.clients.Find(clientId);
        if (client == null)
            throw new Exception("������� � ����� ����� �� ����������");

        appointmentInfo.client = client;
        db.appointmentsInfo.Add(appointmentInfo);
        db.SaveChanges();

        await response.WriteAsJsonAsync(appointmentInfo);
    }
    catch (Exception ex)
    {
        await ReturnError(response, ex);
    }
}

async Task CreatePrescription(HttpRequest request, HttpResponse response)
{
    try
    {
        Prescription? prescription = await request.ReadFromJsonAsync<Prescription>();
        if (prescription == null)
            throw new Exception("������ ����");

        int medicamentId = prescription.medicament.medicament_id;
        int appointmentInfoId = prescription.appointmentInfo.appointmentInfo_id;
        Medicament? medicament = db.Medicaments.Find(medicamentId);
        AppointmentInfo? appointmentInfo = db.appointmentsInfo.Find(appointmentInfoId);
        if (appointmentInfo == null)
            throw new Exception("���������� � ������ �����");
        if (medicament == null)
            throw new Exception("���������� � ����������� �����");
        prescription.medicament = medicament;
        prescription.appointmentInfo = appointmentInfo;

        db.prescriptions.Add(prescription);
        db.SaveChanges();

        await response.WriteAsJsonAsync(prescription);
    }
    catch (Exception ex)
    {
        await ReturnError(response, ex);
    }
}

async Task GetClient(HttpRequest request, HttpResponse response)
{
    try
    {
        int id = GetIdFromPath((string)request.Path);
        Client? client = db.clients.Find(id);
        if (client == null)
            throw new Exception("������� � ����� ����� �� ����������");
        await response.WriteAsJsonAsync(client);
    }
    catch (Exception ex)
    {
        await ReturnError(response, ex);
    }
}

// ������ ������ � �����
void ReplaceText(DocX file, string searchText, string newText)
{
    try
    {
        StringReplaceTextOptions replaceOptions = new StringReplaceTextOptions();
        // ������������� �������� ������� ����� �����
        replaceOptions.SearchValue = searchText;
        // ������������� �������� ������� ����� �������� ��������� ��������
        replaceOptions.NewValue = newText;
        // �������� ��������
        file.ReplaceText(replaceOptions);
    }
    catch (Exception exception) { }
}

async Task CancelHospitalization(HttpRequest request, HttpResponse response)
{
    try
    {
        string path = request.Path;
        // �������� id �������������� 
        int hospitalizationId = GetIdFromPath(path);
        // �������� ��������� �������������� �� ��
        Hospitalization hospitalization = db.hospitalizations.Find(hospitalizationId);
        // ��������� ������� ������ �� �������������
        hospitalization.hospitalizationCancelInfo = path.Split('/')[4];
        // �������� ������ �������������� � ��
        db.hospitalizations.Find(hospitalizationId).Edit(hospitalization);
        db.SaveChanges();
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}

async Task SetHospitalizationDateTime(HttpRequest request, HttpResponse response)
{
    try
    {
        // �������� id ��������������
        int hospitalizationId = GetIdFromPath(request.Path);
        // ��������� ���������� �������������� �� ��
        Hospitalization? hospitalization = db.hospitalizations.Find(hospitalizationId);
        // �������� ���������� �� �������������� � ����� id 
        if (hospitalization == null)
            throw new Exception("�������������� � ����� ����� �� ����������");
        // ������������� ���� � ����� ��������������
        hospitalization.hospitalizationStartDate = Convert.ToDateTime(((string)request.Path).Split('/').Last());
        // �������� ������ �������������� � ��
        db.hospitalizations.Find(hospitalization.hospitalization_id).Edit(hospitalization);
        db.SaveChanges();
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}

async Task GetHospitalizationInfo(HttpRequest request, HttpResponse response)
{
    try
    {
        // ��������� id ��������������
        int hospitalizationId = GetIdFromPath(request.Path);
        // ��������� ���������� �������������� �� ��
        Hospitalization? hospitalization = db.hospitalizations.Find(hospitalizationId);
        // �������� ���������� �� �������������� � ����� id 
        if (hospitalization == null)
            throw new Exception("�������������� � ����� ����� �� ����������");
        // ����������� �������������� � ������� json
        await response.WriteAsJsonAsync(hospitalization);
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}
async Task MedicalCareContract(HttpRequest request, HttpResponse response)
{
    try
    {
        // ��������� id ������� 
        int clientId = GetIdFromPath(request.Path);
        // �������� ���������� ������� �� ��
        Client? client = db.clients.Find(clientId);
        // �������� ���������� �� ������ � ����� id 
        if (client == null)
            throw new Exception("������� � ����� ����� �� ����������");
        // ��������� ������� ���� �� ������ ��������� 
        string fullPath = $"{Directory.GetCurrentDirectory()}/����� �������� �������������� ������� ����������� �����.docx";
        // ��������� ��� � ������ docx
        var doc = DocX.Load(fullPath);

        // ������ ������
        ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd.MM.yyyy"));
        ReplaceText(doc, "<ORG>", organizationName);
        ReplaceText(doc, "<position , full name>", "// // // // // // // // /");
        ReplaceText(doc, "<osnovanie>", "// // // // // // // // /");
        ReplaceText(doc, "<clientFIO>", client.FullName);
        ReplaceText(doc, "<license>", "// // // // // // // // /");
        ReplaceText(doc, "<licenseStartDate>", "// // // // // // // // /");
        ReplaceText(doc, "<licenseEndDate>", "// // // // // // // // /");
        ReplaceText(doc, "<licenseORGNameAddressPhoneNumber>", "// // // // // // // // /");
        ReplaceText(doc, "<licenseORGEndDate>", "// // // // // // // // /");
        ReplaceText(doc, "<services>", "// // // // // // // // /");
        ReplaceText(doc, "<waitingDays>", "// // // // // // // // /");
        ReplaceText(doc, "<position>", "// // // // // // // // /");
        ReplaceText(doc, "<ORGAddress>", "// // // // // // // // /");
        ReplaceText(doc, "<ORGEmail>", "// // // // // // // // /");
        ReplaceText(doc, "<OGRN>", "// // // // // // // // /");
        ReplaceText(doc, "<INN>", "// // // // // // // // /");
        ReplaceText(doc, "<positionGW>", "// // // // // // // // /");
        ReplaceText(doc, "<IO Fam>", "// // // // // // // // /");
        ReplaceText(doc, "<clientAddress>", client.address);
        ReplaceText(doc, "<clientOtherAddresses>", "// // // // // // // // /");
        ReplaceText(doc, "<clientPassport>", $"{client.passportNumberAndSeries} {client.passportGetInfo}");
        ReplaceText(doc, "<clientPhoneNumber>", client.phoneNumder);
        ReplaceText(doc, "<clientIO Fam>", $"{client.firstName[0]}{client.lastName[0]} {client.secondName}");

        // ������� ����� �������� �����
        string fileName = $"������� �������������� ������� ����������� ����� {client.FullName} �� " +
            $"{DateTime.Now.ToString("dd-M-yyyy")}.docx";
        // ��������� ����
        doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/medicalCareContract.docx");

        // ���������� �������� �����
        await response.WriteAsJsonAsync(new { fileName = fileName });
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}

async Task PersonalData(HttpRequest request, HttpResponse response)
{
    try
    {
        // ��������� id ������� 
        int clientId = GetIdFromPath(request.Path);
        // �������� ���������� ������� �� ��
        Client? client = db.clients.Find(clientId);
        // �������� ���������� �� ������ � ����� id 
        if (client == null)
            throw new Exception("������� � ����� ����� �� ����������");
        // ��������� ������� ���� �� ������ ��������� 
        string fullPath = $"{Directory.GetCurrentDirectory()}/����� �������� �� ��������� ������������ ������.docx";
        // ��������� ��� � ������ docx
        var doc = DocX.Load(fullPath);

        // ������ ������
        ReplaceText(doc, "<FIO>", client.FullName);
        ReplaceText(doc, "<passportNumberAndSeries>", client.passportNumberAndSeries);
        ReplaceText(doc, "<passportGetInfo>", client.passportGetInfo);
        ReplaceText(doc, "<address>", client.address);
        ReplaceText(doc, "<ORG>", organizationName);
        ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd MMMM yyyy�."));
        ReplaceText(doc, "<target>", "������������ ������������");

        // ������� ����� �������� �����
        string fileName = $"�������� �� ��������� ������������ ������ {client.FullName} �� {DateTime.Now.ToString("dd-M-yyyy")}.docx";
        // ��������� ����
        doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/personalData.docx");

        // ���������� �������� �����
        await response.WriteAsJsonAsync(new { fileName = fileName });
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}

async Task DecodeQRCode(HttpRequest request, HttpResponse response)
{
    try
    {
        string client_id;
        // �������� ��������� ����
        IFormFile file = request.Form.Files[0];
        // ������� ����� ������
        using (var memoryStream = new MemoryStream())
        {
            // �������� ��������� ���� � �����
            await file.CopyToAsync(memoryStream);
            // �������� ���� �� ������ � bitmap
            Bitmap QRCodeImage = new Bitmap(memoryStream);
            // ������� QRCodeDecoder
            QRCodeDecoder decoder = new QRCodeDecoder();
            // �������������� QR ��� � ���������� ������ � client_id
            client_id = decoder.Decode(new QRCodeBitmapImage(QRCodeImage));
        }
        // �������� ���������� ������� �� ��
        Client? client = db.clients.Find(Convert.ToInt32(client_id));
        // �������� ���������� �� ������ � ����� id 
        if (client == null)
            throw new Exception("������� � ����� ����� �� ����������");
        // ���������� ����������� ������� � ������� json
        await response.WriteAsJsonAsync(client);
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}

async Task CreateQRCode(HttpRequest request, HttpResponse response)
{
    // �������� id �������
    string qrCodeText = GetIdFromPath(request.Path).ToString();

    // ������� QRCodeEncoder
    QRCodeEncoder encoder = new QRCodeEncoder();
    // �������� ����� � QR ���
    Bitmap qrCode = encoder.Encode(qrCodeText);
    // ������� ���� 
    string qrpath = $"{Directory.GetCurrentDirectory()}/wwwroot/qr.png";
    // ������� ����� ������ � ������� ��������
    using (FileStream fileStream = new FileStream(qrpath, FileMode.Create))
    {
        // ��������� QR ��� � ����
        qrCode.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
    }

    await response.WriteAsync("QR ��� ������");
}

async Task CreateImage(HttpRequest request, HttpResponse response)
{
    // �������� ��������� ����
    IFormFile file = request.Form.Files[0];
    // ������� ���� � ����� � ������������ �������� \
    var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/Images";
    // ������� �����
    Directory.CreateDirectory(uploadPath);
    // ���������� �������� �����������
    imageFileName = $"{DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss-FFF")}.{file.FileName.Split('.').Last()}";
    // ������� ������ ����
    string fullPath = $"{uploadPath}/{imageFileName}";
    // ������� ����� ������ � ������� ��������
    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
    {
        // ��������� ���� �������
        await file.CopyToAsync(fileStream);
    }

    await response.WriteAsync("����������� ����������");
}

async Task CreateHospitalization(HttpRequest request, HttpResponse response)
{
    try
    {
        // �������������� ���������� ������ 
        Hospitalization? hospitalization = await request.ReadFromJsonAsync<Hospitalization>();
        // ��������� ��� ������ �� ��� ������
        if (hospitalization == null)
            throw new Exception("����������� ������");
        // �������� ����� � ����� �������� �� �������
        string passportNumberAndSeries = hospitalization.client.passportNumberAndSeries;

        if (db.clients.Any(x => x.passportNumberAndSeries == passportNumberAndSeries))
        {
            // ���� � �� ���� ������ � ����� ��������� �� ����������� ��� � ��������������
            hospitalization.client = db.clients.Where(x => x.passportNumberAndSeries == passportNumberAndSeries).First();
        }
        else
        {
            // ���� � �� ��� ������� � ����� ��������� 
            Client client = hospitalization.client;
            // ����������� ���
            client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
            // ��������� ������� � ��
            db.clients.Add(client);
            db.SaveChanges();
            // ����������� ������� � ��������������
            hospitalization.client = client;
        }
        // ��������� �������������� � ��
        db.hospitalizations.Add(hospitalization);
        db.SaveChanges();
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}

async Task CreateClient(HttpRequest request, HttpResponse response)
{
    try
    {
        if (imageFileName == "")
        {
            throw new Exception("��������� ������ ��� �������� �����������");
        }
        // �������������� ���������� ������ 
        Client? client = await request.ReadFromJsonAsync<Client>();
        // ��������� ��� ������ �� ��� ������
        if (client == null)
            throw new Exception("����������� ������");
        // ����������� ���
        client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
        // ��������� ���� ������ ����������
        client.photoPath = imageFileName;

        // ���� ������ � ����� ��������� ��� ���� � �� ������ ��������
        if (db.clients.Any(x => x.passportNumberAndSeries == client.passportNumberAndSeries))
            throw new Exception("������������ � ����� ���������� ��� ����������");
        // ���� ������ � ����� ����������� ������ ��� ���� � �� ������ ��������
        if (db.clients.Any(x => x.medicalCardNumber == client.medicalCardNumber))
            throw new Exception("������������ � ����� ����������������� ����� ����������� ����� ��� ����������");
        // ��������� ������� � ��
        db.clients.Add(client);
        db.SaveChanges();

        await response.WriteAsync("������ ������� ���������");
    }
    catch (Exception exception)
    {
        // ��� ������ ������� ��������� ����������
        string fullPath = $"{Directory.GetCurrentDirectory()}/clientImages/{imageFileName}";
        File.Delete(fullPath);
        await ReturnError(response, exception);
    }
    imageFileName = "";
}