using Microsoft.AspNetCore.Mvc;
using DataBaseClasses;
using System.Text.RegularExpressions;
using static DataBaseClasses.DBInteract;
using Newtonsoft.Json;
using System.Drawing;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Xceed.Words.NET;
using Xceed.Document.NET;
using ApiApp.Controllers.Json;

namespace ApiApp.Controllers
{
    [Route("appointment")]
    public class AppointmentController : Controller
    {
        private static Random random = new Random();

        /// <summary>
        /// Обработчик создания информации о приеме
        /// </summary>
        /// <param name="appointmentInfo"></param>
        /// <returns></returns>
        [Route("info/create")]
        [HttpPost]
        public IActionResult CreateAppointmentInfo([FromBody] AppointmentInfo appointmentInfo)
        {
            try
            {
                if (appointmentInfo == null)
                    throw new Exception("Информация о приеме пуста");

                int clientId = appointmentInfo.client.client_id;
                Client? client = db.clients.Find(clientId);
                if (client == null)
                    throw new Exception("Клиента с таким кодом не существует");

                appointmentInfo.client = client;
                db.appointmentsInfo.Add(appointmentInfo);
                db.SaveChanges();

                return Ok(appointmentInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message});
            }
        }

        /// <summary>
        /// Обработчик нахождения или создания медикамента
        /// </summary>
        /// <param name="medicament"></param>
        /// <returns></returns>
        [Route("medicament")]
        [HttpPost]
        public IActionResult FindOrCreateMedicament([FromBody] Medicament medicament)
        {
            try
            {
                if (medicament == null)
                    throw new Exception("Название медикамента пусто");
                List<Medicament> medicaments = db.medicaments.Where(x => x.medicamentName == medicament.medicamentName).ToList();

                if (medicaments.Count == 0)
                {
                    db.medicaments.Add(medicament);
                    db.SaveChanges();
                }

                medicament = db.medicaments.Where(x => x.medicamentName == medicament.medicamentName).First();

                return Ok(medicament);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }

        /// <summary>
        /// Обработчик создания направления
        /// </summary>
        /// <param name="prescription"></param>
        /// <returns></returns>
        [Route("prescription")]
        [HttpPost]
        public IActionResult CreatePrescription([FromBody] Prescription prescription)
        {
            try
            {
                if (prescription == null)
                    throw new Exception("Рецепт пуст");

                int medicamentId = prescription.medicament.medicament_id;
                int appointmentInfoId = prescription.appointmentInfo.appointmentInfo_id;
                Medicament? medicament = db.medicaments.Find(medicamentId);
                AppointmentInfo? appointmentInfo = db.appointmentsInfo.Find(appointmentInfoId);
                if (appointmentInfo == null)
                    throw new Exception("Информация о приеме пуста");
                if (medicament == null)
                    throw new Exception("Информация о медикаменте пуста");
                prescription.medicament = medicament;
                prescription.appointmentInfo = appointmentInfo;

                db.prescriptions.Add(prescription);
                db.SaveChanges();

                return Ok(prescription);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }

        /// <summary>
        /// Обработчик сохранения аудио сообщения
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("audio")]
        [HttpPost]
        public IActionResult SaveAudioMessage([FromBody] IFormFile file)
        {
            try
            {
                // путь к папке, где будут храниться файлы
                var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
                // создаем папку для хранения файлов
                Directory.CreateDirectory(uploadPath);

                // формируем путь к файлу в папке uploads
                string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss-FFF")}{random.Next(0, 1000).ToString("G3")}.{file.FileName.Split('.').Last()}";
                string fullPath = $"{uploadPath}/{fileName}";

                // сохраняем файл в папку uploads
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Ok(fileName);
            }
            catch
            {
                return BadRequest(new MessageJson { message = "Не удалось сохранить аудио сообщение" });
            }
        }
    }
}
