using Microsoft.AspNetCore.Mvc;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using ApiApp.Controllers.Json;

namespace ApiApp.Controllers
{
    [Route("hospitalizations")]
    public class HospitalizationController : Controller
    {
        /// <summary>
        /// Обработчик создания госпитализации
        /// </summary>
        /// <param name="hospitalization"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public IActionResult CreateHospitalization([FromBody] Hospitalization hospitalization)
        {
            try
            {
                // проверяем что запрос не был пустой
                if (hospitalization == null)
                    throw new Exception("Некоректные данные");
                // получаем номер и серию паспорта из запроса
                string passportNumberAndSeries = hospitalization.client.passportNumberAndSeries;

                if (db.clients.Any(x => x.passportNumberAndSeries == passportNumberAndSeries))
                {
                    // если в бд есть клиент с таким паспортом то привязываем его к госпитализации
                    hospitalization.client = db.clients.Where(x => x.passportNumberAndSeries == passportNumberAndSeries).First();
                }
                else
                {
                    // если в бд нет клиента с таким паспортом 
                    Client client = hospitalization.client;
                    // привязываем пол
                    client.gender = db.genders.Where(x => x.genderName == client.gender.genderName).First();
                    // добавляем клиента в бд
                    db.clients.Add(client);
                    db.SaveChanges();
                    // привязываем клиента к госпитализации
                    hospitalization.client = client;
                }
                // добавляем госпитализацию в бд
                db.hospitalizations.Add(hospitalization);
                db.SaveChanges();

                return Ok(new MessageJson { message = "Данные успешно загружены" });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }

        /// <summary>
        /// Обработчик получения клиента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetHospitalization(int id)
        {
            try
            {
                // получение экземпляра госпитализации из бд
                Hospitalization? hospitalization = db.hospitalizations.Find(id);
                // проверка существует ли госпитализация с таким id 
                if (hospitalization == null)
                    throw new Exception("Госпитацизации с таким кодом не существует");
                // возвращение госпитализации в формате json
                return Ok(hospitalization);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }

        /// <summary>
        /// Обработчик отмены госпитализации
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelInfo"></param>
        /// <returns></returns>
        [Route("{id}/cancel")]
        [HttpPost]
        public IActionResult CancelHospitalization(int id, [FromBody] MessageJson cancelInfo)
        {
            try
            {
                // получаем экземпляр госпитализации из бд
                Hospitalization hospitalization = db.hospitalizations.Find(id);
                // заполняем причину отказа от госитализации
                hospitalization.hospitalizationCancelInfo = cancelInfo.message;
                // изменяем запись госпитализации в бд
                db.hospitalizations.Find(id).Edit(hospitalization);
                db.SaveChanges();

                return Ok(new MessageJson { message = "Данные успешно загружены" });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }

        /// <summary>
        /// Обработчик установки даты и времени
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [Route("{id}/date/{date}")]
        [HttpGet]
        public IActionResult SetHospitalizationDate(int id, DateTime date)
        {
            try
            {
                // получение экземпляра госпитализации из бд
                Hospitalization? hospitalization = db.hospitalizations.Find(id);
                // проверка существует ли госпитализация с таким id 
                if (hospitalization == null)
                    throw new Exception("Госпитацизации с таким кодом не существует");
                // устанавливаем дату и время госпитализации
                hospitalization.hospitalizationStartDate = date;
                // изменяем запись госпитализации в бд
                db.hospitalizations.Find(id).Edit(hospitalization);
                db.SaveChanges();

                return Ok(new MessageJson { message = "Данные успешно загружены" });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }
    }
}
