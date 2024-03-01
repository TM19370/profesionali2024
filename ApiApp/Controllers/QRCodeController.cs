using Microsoft.AspNetCore.Mvc;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using System.Drawing;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using ApiApp.Controllers.Json;

namespace ApiApp.Controllers
{
    [Route("qrcode")]
    public class QRCodeController : Controller
    {
        /// <summary>
        /// Обработчик создания QR кода
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}/create")]
        [HttpGet]
        public IActionResult CreateQRCode(string id)
        {
            try
            {
                // создаем QRCodeEncoder
                QRCodeEncoder encoder = new QRCodeEncoder();
                // кодируем текст в QR код
                Bitmap qrCode = encoder.Encode(id);
                // создаем путь 
                string qrpath = $"{Directory.GetCurrentDirectory()}/wwwroot/qr.png";
                // создаем поток данных с режимом создания
                using (FileStream fileStream = new FileStream(qrpath, FileMode.Create))
                {
                    // сохраняем QR код в файл
                    qrCode.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
                }

                return Ok(new MessageJson { message = "QR код создан" });
            }
            catch
            {
                return BadRequest(new MessageJson { message = "Не удалось создать QR код" });
            }
        }

        /// <summary>
        /// Обработчик декодирования QR кода
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("decode")]
        [HttpPost]
        public IActionResult DecodeQRCode([FromBody] IFormFile file)
        {
            try
            {
                string client_id;
                // создаем поток данных
                using (var memoryStream = new MemoryStream())
                {
                    // копируем полученый файл в поток
                    file.CopyTo(memoryStream);
                    // копируем файл из потока в bitmap
                    Bitmap QRCodeImage = new Bitmap(memoryStream);
                    // создаем QRCodeDecoder
                    QRCodeDecoder decoder = new QRCodeDecoder();
                    // расшифровываем QR код и записываем данные в client_id
                    client_id = decoder.Decode(new QRCodeBitmapImage(QRCodeImage));
                }
                // получние экземпляра клиента из бд
                Client? client = db.clients.Find(Convert.ToInt32(client_id));
                // проверка существует ли клиент с таким id 
                if (client == null)
                    throw new Exception("Клиента с таким кодом не существует");
                // возвращаем полученного клиента в формате json

                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }
    }
}
