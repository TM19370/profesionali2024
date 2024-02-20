using System.Text.RegularExpressions;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
Random random = new Random();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.Write($"{DateTime.Now.ToString("HH:mm:ss:fff")} Request path: ");
    Console.ResetColor();
    Console.WriteLine(path);

    if (path == "/" || path == "/favicon.ico")//������ ������ ��������
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
    Console.WriteLine($"Error: {exception.Message}");
    response.StatusCode = 400;
    await response.WriteAsJsonAsync(new { error = exception.Message });
}
/* ��� ������ ���������
async Task<T> GetFromDB<T>(HttpRequest request, HttpResponse response)
{
    

    return default;
}
*/
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