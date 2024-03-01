using Microsoft.AspNetCore.Mvc;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using ApiApp.Controllers.Json;

namespace ApiApp.Controllers
{
    [Route("clients")]
    public class ClientController : Controller
    {
        private static string imageFileName = "";

        /// <summary>
        /// Обработчик создания клиента
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public IActionResult Create([FromBody] Client client)
        {
            try
            {
                if (imageFileName == "")
                {
                    throw new Exception("Произошла ошибка при загрузке изображения");
                }
                // проверяем что запрос не был пустой
                if (client == null)
                    throw new Exception("Некоректные данные");
                // привязываем пол
                client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
                // заполняем поле адресс фотографии
                client.photoPath = imageFileName;

                // если клиент с таким паспортом уже есть в бд отмена создания
                if (db.clients.Any(x => x.passportNumberAndSeries == client.passportNumberAndSeries))
                    throw new Exception("Пользователь с таким пасспортом уже существует");
                // если клиент с такой медицинской картой уже есть в бд отмена создания
                if (db.clients.Any(x => x.medicalCardNumber == client.medicalCardNumber))
                    throw new Exception("Пользователь с таким идентификационным кодом медицинской карты уже существует");
                // добавляем клиента в бд
                db.clients.Add(client);
                db.SaveChanges();

                imageFileName = "";

                return Ok(new MessageJson { message = "Данные успешно загружены" });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }

        /// <summary>
        /// Обработчик создания фотографии
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("image")]
        [HttpPost]
        public IActionResult Image([FromBody] IFormFile file)
        {
            try
            {
                // создаем путь в папку с фотографиями клиентов
                var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/Images";
                // создаем папки
                Directory.CreateDirectory(uploadPath);
                // генерируем название изображения
                imageFileName = $"{DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss-FFF")}.{file.FileName.Split('.').Last()}";
                // создаем полный путь
                string fullPath = $"{uploadPath}/{imageFileName}";
                // создаем поток данных с режимом создания
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    // сохраняем фото клиента
                    file.CopyTo(fileStream);
                }

                return Ok(new MessageJson { message = "Изображение загружено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = "Не удалость загрузить изображение" });
            }
        }

        /// <summary>
        /// Обработчик получения клиента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetClient(int id)
        {
            try
            {
                Client? client = db.clients.Find(id);
                if (client == null)
                    throw new Exception("Клиента с таким кодом не существует");

                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }
    }
}
