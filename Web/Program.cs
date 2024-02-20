using System.Drawing;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Xceed.Words.NET;
using Xceed.Document.NET;
using System.Text.RegularExpressions;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;

string organizationName = "ГКБ Большие Кабаны";
string imageFileName = "";

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
        //установка даты и времени госпитализации
        await SetHospitalizationDateTime(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/hospitalization/\d{1,}") && request.Method == "GET")
    {
        //получение информации о госпитализации
        await GetHospitalizationInfo(request, response);
    }
    else if(path == "/api/hospitalization" && request.Method == "POST")
    {
        //создание госпитализации
        await CreateHospitalization(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/cancelHospitalization/\d{1,}/\w{1,}") && request.Method == "POST")
    {
        //отмена госпитализации
        await CancelHospitalization(request, response);
    }
    else if (path == "/api/clients" && request.Method == "POST")
    {
        //создание клиента
        await CreateClient(request, response);
    }
    else if (path == "/api/image" && request.Method == "POST")
    {
        //создание изображения
        await CreateImage(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/QRCode/\d{1,}") && request.Method == "GET")
    {
        //создание QR кода
        await CreateQRCode(request, response);
    }
    else if (path == "/api/DecodeQRCode" && request.Method == "POST")
    {
        //расшифровка QR кода
        await DecodeQRCode(request, response);
    }
    else if(Regex.IsMatch(path, @"/api/personalData/\d{1,}") && request.Method == "GET")
    {
        //заполнение согласия на обработку персональных даннных
        await PersonalData(request, response);
    }
    else if (Regex.IsMatch(path, @"/api/medicalCareContract/\d{1,}") && request.Method == "GET")
    {
        //заполнение договора предоставления платных медицинских услуг
        await MedicalCareContract(request, response);
    }
    else
    {
        //Api с таким адресом не существует
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { error = "API не найдено" });
    }
});

app.Run();

// Получение id из пути
int GetIdFromPath(string path)
{
    return Convert.ToInt32(path.Split('/')[3]);
}

// Возвращение ошибки
async Task ReturnError(HttpResponse response, Exception exception)
{
    response.StatusCode = 400;
    await response.WriteAsJsonAsync(new { error = exception.Message });
}

//Замена текста в файле
void ReplaceText(DocX file, string searchText, string newText)
{
    try
    {
        StringReplaceTextOptions replaceOptions = new StringReplaceTextOptions();
        //устанавливаем значение которое нужно найти
        replaceOptions.SearchValue = searchText;
        //устанавливаем значание которое будет заменять найденное значение
        replaceOptions.NewValue = newText;
        //заменяем значение
        file.ReplaceText(replaceOptions);
    }
    catch (Exception exception) { }
}

async Task CancelHospitalization(HttpRequest request, HttpResponse response)
{
    try
    {
        string path = request.Path;
        //получаем id госпитализации 
        int hospitalizationId = GetIdFromPath(path);
        //получаем экземпляр госпитализации из бд
        Hospitalization hospitalization = db.hospitalizations.Find(hospitalizationId);
        //заполняем причину отказа от госитализации
        hospitalization.hospitalizationCancelInfo = path.Split('/')[4];
        //изменяем запись госпитализации в бд
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
        //получаем id госпитализации
        int hospitalizationId = GetIdFromPath(request.Path);
        //получение экземпляра госпитализации из бд
        Hospitalization? hospitalization = db.hospitalizations.Find(hospitalizationId);
        //проверка существует ли госпитализация с таким id 
        if (hospitalization == null)
            throw new Exception("Госпитацизации с таким кодом не существует");
        //устанавливаем дату и время госпитализации
        hospitalization.hospitalizationStartDate = Convert.ToDateTime(((string)request.Path).Split('/').Last());
        //изменяем запись госпитализации в бд
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
        //получение id госпитализации
        int hospitalizationId = GetIdFromPath(request.Path);
        //получение экземпляра госпитализации из бд
        Hospitalization? hospitalization = db.hospitalizations.Find(hospitalizationId);
        //проверка существует ли госпитализация с таким id 
        if (hospitalization == null)
            throw new Exception("Госпитацизации с таким кодом не существует");
        //возвращение госпитализации в формате json
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
        //получение id клиента 
        int clientId = GetIdFromPath(request.Path);
        //получние экземпляра клиента из бд
        Client? client = db.clients.Find(clientId);
        //проверка существует ли клиент с таким id 
        if (client == null)
            throw new Exception("Клиента с таким кодом не существует");
        //получение полного пути до бланка документа 
        string fullPath = $"{Directory.GetCurrentDirectory()}/бланк договора предоставления платных медицинских услуг.docx";
        //открываем его с помощю docx
        var doc = DocX.Load(fullPath);

        //замена текста
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

        //создаем новое название файла
        string fileName = $"Договор предоставления платных медицинских услуг {client.FullName} от " +
            $"{DateTime.Now.ToString("dd-M-yyyy")}.docx";
        //сохраняем файл
        doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/medicalCareContract.docx");

        //возвращаем название файла
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
        //получение id клиента 
        int clientId = GetIdFromPath(request.Path);
        //получние экземпляра клиента из бд
        Client? client = db.clients.Find(clientId);
        //проверка существует ли клиент с таким id 
        if (client == null)
            throw new Exception("Клиента с таким кодом не существует");
        //получение полного пути до бланка документа 
        string fullPath = $"{Directory.GetCurrentDirectory()}/бланк согласия на обработку персональных данных.docx";
        //открываем его с помощю docx
        var doc = DocX.Load(fullPath);

        //замена текста
        ReplaceText(doc, "<FIO>", client.FullName);
        ReplaceText(doc, "<passportNumberAndSeries>", client.passportNumberAndSeries);
        ReplaceText(doc, "<passportGetInfo>", client.passportGetInfo);
        ReplaceText(doc, "<address>", client.address);
        ReplaceText(doc, "<ORG>", organizationName);
        ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd MMMM yyyyг."));
        ReplaceText(doc, "<target>", "медицинского обслуживания");

        //создаем новое название файла
        string fileName = $"Согласие на обработку персоняльных данных {client.FullName} от {DateTime.Now.ToString("dd-M-yyyy")}.docx";
        //сохраняем файл
        doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/personalData.docx");

        //возвращаем название файла
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
        //выбираем полученый файл
        IFormFile file = request.Form.Files[0];
        //создаем поток данных
        using (var memoryStream = new MemoryStream())
        {
            //копируем полученый файл в поток
            await file.CopyToAsync(memoryStream);
            //копируем файл из потока в bitmap
            Bitmap QRCodeImage = new Bitmap(memoryStream);
            //создаем QRCodeDecoder
            QRCodeDecoder decoder = new QRCodeDecoder();
            //расшифровываем QR код и записываем данные в client_id
            client_id = decoder.Decode(new QRCodeBitmapImage(QRCodeImage));
        }
        //получние экземпляра клиента из бд
        Client? client = db.clients.Find(Convert.ToInt32(client_id));
        //проверка существует ли клиент с таким id 
        if (client == null)
            throw new Exception("Клиента с таким кодом не существует");
        //возвращаем полученного клиента в формате json
        await response.WriteAsJsonAsync(client);
    }
    catch (Exception exception)
    {
        await ReturnError(response, exception);
    }
}

async Task CreateQRCode(HttpRequest request, HttpResponse response)
{
    //получаем id клиента
    string qrCodeText = GetIdFromPath(request.Path).ToString();

    //создаем QRCodeEncoder
    QRCodeEncoder encoder = new QRCodeEncoder();
    //кодируем текст в QR код
    Bitmap qrCode = encoder.Encode(qrCodeText);
    //создаем путь 
    string qrpath = $"{Directory.GetCurrentDirectory()}/wwwroot/qr.png";
    //создаем поток данных с режимом создания
    using (FileStream fileStream = new FileStream(qrpath, FileMode.Create))
    {
        //сохраняем QR код в файл
        qrCode.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
    }

    await response.WriteAsync("QR код создан");
}

async Task CreateImage(HttpRequest request, HttpResponse response)
{
    //выбираем полученый файл
    IFormFile file = request.Form.Files[0];
    //создаем путь в папку с фотографиями клиентов \
    var uploadPath = $"{Directory.GetCurrentDirectory()}/clientImages";
    //создаем папки
    Directory.CreateDirectory(uploadPath);
    //генерируем название изображения
    imageFileName = $"{DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss-FFF")}.{file.FileName.Split('.').Last()}";
    //создаем полный путь
    string fullPath = $"{uploadPath}/{imageFileName}";
    //создаем поток данных с режимом создания
    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
    {
        //сохраняем фото клиента
        await file.CopyToAsync(fileStream);
    }

    await response.WriteAsync("Изображение загруженно");
}

async Task CreateHospitalization(HttpRequest request, HttpResponse response)
{
    try
    {
        //расшифровываем полученный запрос 
        Hospitalization? hospitalization = await request.ReadFromJsonAsync<Hospitalization>();
        //проверяем что запрос не был пустой
        if(hospitalization == null)
            throw new Exception("Некоректные данные");
        //получаем номер и серию паспорта из запроса
        string passportNumberAndSeries = hospitalization.client.passportNumberAndSeries;

        if (db.clients.Any(x => x.passportNumberAndSeries == passportNumberAndSeries))
        {
            //если в бд есть клиент с таким паспортом то привязываем его к госпитализации
            hospitalization.client = db.clients.Find(hospitalization.client.client_id);
        }
        else
        {
            //если в бд нет клиента с таким паспортом 
            Client client = hospitalization.client;
            //привязываем пол
            client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
            //добавляем клиента в бд
            db.clients.Add(client);
            db.SaveChanges();
            //привязываем клиента к госпитализации
            hospitalization.client = client;
        }
        //добавляем госпитализацию в бд
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
            throw new Exception("Произошла ошибка при загрузке изображения");
        }
        //расшифровываем полученный запрос 
        Client? client = await request.ReadFromJsonAsync<Client>();
        //проверяем что запрос не был пустой
        if (client == null)
            throw new Exception("Некоректные данные");
        //привязываем пол
        client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
        //заполняем поле адресс фотографии
        client.photoPath = imageFileName;

        //если клиент с таким паспортом уже есть в бд отмена создания
        if (db.clients.Any(x => x.passportNumberAndSeries == client.passportNumberAndSeries))
            throw new Exception("Пользователь с таким пасспортом уже существует");
        //если клиент с такой медицинской картой уже есть в бд отмена создания
        if (db.clients.Any(x => x.medicalCardNumber == client.medicalCardNumber))
            throw new Exception("Пользователь с таким идентификационным кодом медицинской карты уже существует");
        //добавляем клиента в бд
        db.clients.Add(client);
        db.SaveChanges();

        await response.WriteAsync("Данные успешно загружены");
    }
    catch (Exception exception)
    {
        //при ошибке удаляем созданную фотографию
        string fullPath = $"{Directory.GetCurrentDirectory()}/clientImages/{imageFileName}";
        File.Delete(fullPath);
        await ReturnError(response, exception);
    }
    imageFileName = "";
}