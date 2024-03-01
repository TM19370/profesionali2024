using Microsoft.AspNetCore.Mvc;
using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using Xceed.Words.NET;
using Xceed.Document.NET;
using ApiApp.Controllers.Json;

namespace ApiApp.Controllers
{
    [Route("document")]
    public class DocumentsController : Controller
    {
        private string organizationName = "МИС ГКБ Большие кабаны";

        // Замена текста в файле
        void ReplaceText(DocX file, string searchText, string newText)
        {
            try
            {
                StringReplaceTextOptions replaceOptions = new StringReplaceTextOptions();
                // устанавливаем значение которое нужно найти
                replaceOptions.SearchValue = searchText;
                // устанавливаем значание которое будет заменять найденное значение
                replaceOptions.NewValue = newText;
                // заменяем значение
                file.ReplaceText(replaceOptions);
            }
            catch (Exception exception) { }
        }

        /// <summary>
        /// Обработчик получения согласия на обработку персональных данных
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("personaldata/{id}")]
        [HttpGet]
        public IActionResult GetPersonalData(int id)
        {
            try
            {
                // получние экземпляра клиента из бд
                Client? client = db.clients.Find(id);
                // проверка существует ли клиент с таким id 
                if (client == null)
                    throw new Exception("Клиента с таким кодом не существует");
                // получение полного пути до бланка документа 
                string fullPath = $"{Directory.GetCurrentDirectory()}/бланк согласия на обработку персональных данных.docx";
                // открываем его с помощю docx
                var doc = DocX.Load(fullPath);

                // замена текста
                ReplaceText(doc, "<FIO>", client.fullName.GetFullName);
                ReplaceText(doc, "<passportNumberAndSeries>", client.passportNumberAndSeries);
                ReplaceText(doc, "<passportGetInfo>", client.passportGetInfo);
                ReplaceText(doc, "<address>", client.address);
                ReplaceText(doc, "<ORG>", organizationName);
                ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd MMMM yyyyг."));
                ReplaceText(doc, "<target>", "медицинского обслуживания");

                // создаем новое название файла
                string fileName = $"Согласие на обработку персоняльных данных {client.fullName.GetFullName} от {DateTime.Now.ToString("dd-M-yyyy")}.docx";
                // сохраняем файл
                doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/personalData.docx");

                // возвращаем название файла
                return Ok(new MessageJson { message = fileName });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }

        /// <summary>
        /// Обработчик получения договора предоставления платных медицинских услуг
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("medicalcarecontract/{id}")]
        [HttpGet]
        public IActionResult GetMedicalCareContract(int id)
        {
            try
            {
                // получние экземпляра клиента из бд
                Client? client = db.clients.Find(id);
                // проверка существует ли клиент с таким id 
                if (client == null)
                    throw new Exception("Клиента с таким кодом не существует");
                // получение полного пути до бланка документа 
                string fullPath = $"{Directory.GetCurrentDirectory()}/бланк договора предоставления платных медицинских услуг.docx";
                // открываем его с помощю docx
                var doc = DocX.Load(fullPath);

                // замена текста
                ReplaceText(doc, "<currentDate>", DateTime.Now.ToString("dd.MM.yyyy"));
                ReplaceText(doc, "<ORG>", organizationName);
                ReplaceText(doc, "<position , full name>", "// // // // // // // // /");
                ReplaceText(doc, "<osnovanie>", "// // // // // // // // /");
                ReplaceText(doc, "<clientFIO>", client.fullName.GetFullName);
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
                ReplaceText(doc, "<clientIO Fam>", $"{client.fullName.firstName[0]}{client.fullName.lastName[0]} {client.fullName.secondName}");

                // создаем новое название файла
                string fileName = $"Договор предоставления платных медицинских услуг {client.fullName.GetFullName} от " +
                    $"{DateTime.Now.ToString("dd-M-yyyy")}.docx";
                // сохраняем файл
                doc.SaveAs(Directory.GetCurrentDirectory() + "/wwwroot/medicalCareContract.docx");

                // возвращаем название файла
                return Ok(new MessageJson { message = fileName });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageJson { message = ex.Message });
            }
        }
    }
}
