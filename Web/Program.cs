using System.Drawing;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Xceed.Words.NET;
using Xceed.Document.NET;
using System.Text.RegularExpressions;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;

string organizationName = "��� ������� ������";
string imageFileName = "";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    if (path == "/")//������ ������ ��������
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/html/registration.html");
    }
    else if (Regex.IsMatch(path, @"/api/hospitalization/\d{1,}/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}") && request.Method == "POST")
    {
        //��������� ���� � ������� ��������������
        await SetHospitalizationDateTime(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/hospitalization/\d{1,}") && request.Method == "GET")
    {
        //��������� ���������� � ��������������
        await GetHospitalizationInfo(request, response);
    }
    else if(path == "/api/hospitalization" && request.Method == "POST")
    {
        //�������� ��������������
        await CreateHospitalization(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/cancelHospitalization/\d{1,}/\w{1,}") && request.Method == "POST")
    {
        //������ ��������������
        await CancelHospitalization(request, response);
    }
    else if (path == "/api/clients" && request.Method == "POST")
    {
        //�������� �������
        await CreateClient(request, response);
    }
    else if (path == "/api/image" && request.Method == "POST")
    {
        //�������� �����������
        await CreateImage(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/QRCode/\d{1,}") && request.Method == "GET")
    {
        //�������� QR ����
        await CreateQRCode(request, response);
    }
    else if (path == "/api/DecodeQRCode" && request.Method == "POST")
    {
        //����������� QR ����
        await DecodeQRCode(request, response);
    }
    else if(Regex.IsMatch(path, @"/api/personalData/\d{1,}") && request.Method == "GET")
    {
        //���������� �������� �� ��������� ������������ �������
        await PersonalData(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/medicalCareContract/\d{1,}") && request.Method == "GET")
    {
        //���������� �������� �������������� ������� ����������� �����
        await MedicalCareContract(request, response);
    }
    else
    {
        //Api � ����� ������� �� ����������
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { error = "API �� �������" });
    }
});

app.Run();

// ��������� id �� ����
int GetIdFromPath(string path)
{
    return Convert.ToInt32(path.Split('/')[3]);
}

// ����������� ������
async Task ReturnError(HttpResponse response, Exception exception)
{
    response.StatusCode = 400;
    await response.WriteAsJsonAsync(new { error = exception.Message });
}

//������ ������ � �����
void ReplaceText(DocX file, string searchText, string newText)
{
    try
    {
        StringReplaceTextOptions replaceOptions = new StringReplaceTextOptions();
        //������������� �������� ������� ����� �����
        replaceOptions.SearchValue = searchText;
        //������������� �������� ������� ����� �������� ��������� ��������
        replaceOptions.NewValue = newText;
        //�������� ��������
        file.ReplaceText(replaceOptions);
    }
    catch (Exception exception) { }
}

async Task CancelHospitalization(HttpRequest request, HttpResponse response)
{
    try
    {
        string path = request.Path;
        //�������� id �������������� 
        int hospitalizationId = GetIdFromPath(path);
        //�������� ��������� �������������� �� ��
        Hospitalization hospitalization = db.hospitalizations.Find(hospitalizationId);
        //��������� ������� ������ �� �������������
        hospitalization.hospitalizationCancelInfo = path.Split('/')[4];
        //�������� ������ �������������� � ��
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
        //�������� id ��������������
        int hospitalizationId = GetIdFromPath(request.Path);
        //��������� ���������� �������������� �� ��
        Hospitalization? hospitalization = db.hospitalizations.Find(hospitalizationId);
        //�������� ���������� �� �������������� � ����� id 
        if (hospitalization == null)
            throw new Exception("�������������� � ����� ����� �� ����������");
        //������������� ���� � ����� ��������������
        hospitalization.hospitalizationStartDate = Convert.ToDateTime(((string)request.Path).Split('/').Last());
        //�������� ������ �������������� � ��
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
        //��������� id ��������������
        int hospitalizationId = GetIdFromPath(request.Path);
        //��������� ���������� �������������� �� ��
        Hospitalization? hospitalization = db.hospitalizations.Find(hospitalizationId);
        //�������� ���������� �� �������������� � ����� id 
        if (hospitalization == null)
            throw new Exception("�������������� � ����� ����� �� ����������");
        //����������� �������������� � ������� json
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
        //��������� id ������� 
        int clientId = GetIdFromPath(request.Path);
        //�������� ���������� ������� �� ��
        Client? client = db.clients.Find(clientId);
        //�������� ���������� �� ������ � ����� id 
        if (client == null)
            throw new Exception("������� � ����� ����� �� ����������");
        //��������� ������� ���� �� ������ ��������� 
        string fullPath = $"{Directory.GetCurrentDirectory()}/����� �������� �������������� ������� ����������� �����.docx";
        //��������� ��� � ������ docx
        var doc = DocX.Load(fullPath);

        //������ ������
        ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd.MM.yyyy"));
        ReplaceText(doc, "<ORG>", organizationName);
        ReplaceText(doc, "<position , full name>", "/////////////////");
        ReplaceText(doc, "<osnovanie>", "/////////////////");
        ReplaceText(doc, "<clientFIO>", client.FullName);
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

        //������� ����� �������� �����
        string fileName = $"������� �������������� ������� ����������� ����� {client.FullName} �� " +
            $"{DateTime.Now.ToString("dd-M-yyyy")}.docx";
        //��������� ����
        doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/medicalCareContract.docx");

        //���������� �������� �����
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
        //��������� id ������� 
        int clientId = GetIdFromPath(request.Path);
        //�������� ���������� ������� �� ��
        Client? client = db.clients.Find(clientId);
        //�������� ���������� �� ������ � ����� id 
        if (client == null)
            throw new Exception("������� � ����� ����� �� ����������");
        //��������� ������� ���� �� ������ ��������� 
        string fullPath = $"{Directory.GetCurrentDirectory()}/����� �������� �� ��������� ������������ ������.docx";
        //��������� ��� � ������ docx
        var doc = DocX.Load(fullPath);

        //������ ������
        ReplaceText(doc, "<FIO>", client.FullName);
        ReplaceText(doc, "<passportNumberAndSeries>", client.passportNumberAndSeries);
        ReplaceText(doc, "<passportGetInfo>", client.passportGetInfo);
        ReplaceText(doc, "<address>", client.address);
        ReplaceText(doc, "<ORG>", organizationName);
        ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd MMMM yyyy�."));
        ReplaceText(doc, "<target>", "������������ ������������");

        //������� ����� �������� �����
        string fileName = $"�������� �� ��������� ������������ ������ {client.FullName} �� {DateTime.Now.ToString("dd-M-yyyy")}.docx";
        //��������� ����
        doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/personalData.docx");

        //���������� �������� �����
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
        //�������� ��������� ����
        IFormFile file = request.Form.Files[0];
        //������� ����� ������
        using (var memoryStream = new MemoryStream())
        {
            //�������� ��������� ���� � �����
            await file.CopyToAsync(memoryStream);
            //�������� ���� �� ������ � bitmap
            Bitmap QRCodeImage = new Bitmap(memoryStream);
            //������� QRCodeDecoder
            QRCodeDecoder decoder = new QRCodeDecoder();
            //�������������� QR ��� � ���������� ������ � client_id
            client_id = decoder.Decode(new QRCodeBitmapImage(QRCodeImage));
        }
        //�������� ���������� ������� �� ��
        Client? client = db.clients.Find(Convert.ToInt32(client_id));
        //�������� ���������� �� ������ � ����� id 
        if (client == null)
            throw new Exception("������� � ����� ����� �� ����������");
        //���������� ����������� ������� � ������� json
        await response.WriteAsJsonAsync(client);
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}

async Task CreateQRCode(HttpRequest request, HttpResponse response)
{
    //�������� id �������
    string qrCodeText = GetIdFromPath(request.Path).ToString();

    //������� QRCodeEncoder
    QRCodeEncoder encoder = new QRCodeEncoder();
    //�������� ����� � QR ���
    Bitmap qrCode = encoder.Encode(qrCodeText);
    //������� ���� 
    string qrpath = $"{Directory.GetCurrentDirectory()}/wwwroot/qr.png";
    //������� ����� ������ � ������� ��������
    using (FileStream fileStream = new FileStream(qrpath, FileMode.Create))
    {
        //��������� QR ��� � ����
        qrCode.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
    }

    await response.WriteAsync("QR ��� ������");
}

async Task CreateImage(HttpRequest request, HttpResponse response)
{
    //�������� ��������� ����
    IFormFile file = request.Form.Files[0];
    //������� ���� � ����� � ������������ �������� \
    var uploadPath = $"{Directory.GetCurrentDirectory()}/clientImages";
    //������� �����
    Directory.CreateDirectory(uploadPath);
    //���������� �������� �����������
    imageFileName = $"{DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss-FFF")}.{file.FileName.Split('.').Last()}";
    //������� ������ ����
    string fullPath = $"{uploadPath}/{imageFileName}";
    //������� ����� ������ � ������� ��������
    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
    {
        //��������� ���� �������
        await file.CopyToAsync(fileStream);
    }

    await response.WriteAsync("����������� ����������");
}

async Task CreateHospitalization(HttpRequest request, HttpResponse response)
{
    try
    {
        //�������������� ���������� ������ 
        Hospitalization? hospitalization = await request.ReadFromJsonAsync<Hospitalization>();
        //��������� ��� ������ �� ��� ������
        if(hospitalization == null)
            throw new Exception("����������� ������");
        //�������� ����� � ����� �������� �� �������
        string passportNumberAndSeries = hospitalization.client.passportNumberAndSeries;

        if (db.clients.Any(x => x.passportNumberAndSeries == passportNumberAndSeries))
        {
            //���� � �� ���� ������ � ����� ��������� �� ����������� ��� � ��������������
            hospitalization.client = db.clients.Find(hospitalization.client.client_id);
        }
        else
        {
            //���� � �� ��� ������� � ����� ��������� 
            Client client = hospitalization.client;
            //����������� ���
            client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
            //��������� ������� � ��
            db.clients.Add(client);
            db.SaveChanges();
            //����������� ������� � ��������������
            hospitalization.client = client;
        }
        //��������� �������������� � ��
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
        //�������������� ���������� ������ 
        Client? client = await request.ReadFromJsonAsync<Client>();
        //��������� ��� ������ �� ��� ������
        if (client == null)
            throw new Exception("����������� ������");
        //����������� ���
        client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
        //��������� ���� ������ ����������
        client.photoPath = imageFileName;

        //���� ������ � ����� ��������� ��� ���� � �� ������ ��������
        if (db.clients.Any(x => x.passportNumberAndSeries == client.passportNumberAndSeries))
            throw new Exception("������������ � ����� ���������� ��� ����������");
        //���� ������ � ����� ����������� ������ ��� ���� � �� ������ ��������
        if (db.clients.Any(x => x.medicalCardNumber == client.medicalCardNumber))
            throw new Exception("������������ � ����� ����������������� ����� ����������� ����� ��� ����������");
        //��������� ������� � ��
        db.clients.Add(client);
        db.SaveChanges();

        await response.WriteAsync("������ ������� ���������");
    }
    catch (Exception exception)
    {
        //��� ������ ������� ��������� ����������
        string fullPath = $"{Directory.GetCurrentDirectory()}/clientImages/{imageFileName}";
        File.Delete(fullPath);
        await ReturnError(response, exception);
    }
    imageFileName = "";
}