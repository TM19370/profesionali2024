using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static МИС__ГКБ_Большие_Кабаны_.DBInteract;

namespace МИС__ГКБ_Большие_Кабаны_
{
    /// <summary>
    /// Логика взаимодействия для PatientsWindow.xaml
    /// </summary>
    public partial class PatientsWindow : Window
    {
        public PatientsWindow()
        {
            InitializeComponent();
            generateClients();
        }

        Random random = new Random();
        string[,] names = { { "Анна", "Юля", "Татьяна", "Вика", "Алина", "Валентина", "Жанна", "Агния", "Алла", "Берта" },
            { "Тимофей", "Данил", "Олег", "Юра", "Артем", "Саша", "Роман", "Никита", "Дмитрий", "Егор" } };
        string[] sNames = { "Тимофеев", "Данилов", "Олегов", "Юрьев", "Артемов", "Сашев", "Романов", "Никитов", "Дмитриев", "Егоров" };
        string[] streets = { "Пушкина", "Московская", "Советская" };
        int mImage = 1;
        int fImage = 1;
        void generateClients()
        {
            List<Client> clients = new List<Client>();
            for (int i = 0; i < 100; i++)
            {
                int gender;
                if (clients.Where(x => x.gender.genderName == "мужской").Count() >= 35)
                    gender = 0;
                else if(clients.Where(x => x.gender.genderName == "женский").Count() >= 65)
                    gender = 1;
                else
                    gender = random.Next(0, 2);
                string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss-FFF")}-{random.Next(0,1000).ToString("G3")}.jpg";

                var uploadPath = $"{Directory.GetCurrentDirectory()}/clientImages";
                Directory.CreateDirectory(uploadPath);

                if (gender == 0)
                    File.Copy($@"D:\F\{fImage}.jpg", uploadPath + fileName);
                else
                    File.Copy($@"D:\M\{mImage}.jpg", uploadPath + fileName);

                DateTime birthDate = DateTime.Parse($"{random.Next(1, 29)}.{random.Next(1, 13)}.{random.Next(1990, 2016)}");

                //копировать изображения
                Client newClient = new Client()
                {
                    photoPath = fileName,
                    firstName = names[gender, random.Next(0, 10)],
                    secondName = sNames[random.Next(0, 10)] + (gender == 0 ? "а" : ""),
                    lastName = sNames[random.Next(0, 10)] + (gender == 0 ? "на" : "ич"),
                    passportNumberAndSeries = $"{random.Next(1, 100).ToString("G2")}{random.Next(1, 100).ToString("G2")}{random.Next(1, 1000000).ToString("G6")}",
                    birthDate = birthDate,
                    gender = db.genders.Find(gender == 0? "женский": "мужской"),
                    address = $"г. Большие Кабаны ул. {streets[random.Next(0,3)]} д. {random.Next(1,100).ToString("G2")} кв. {random.Next(1,100).ToString("G2")}",
                    phoneNumder = $"+7{random.Next(0, 1000).ToString("G3")}{random.Next(0, 1000).ToString("G3")}{random.Next(0, 100).ToString("G2")}{random.Next(0, 100).ToString("G2")}",
                    email = $"{random.Next(0, 1000).ToString("G3")}{DateTime.Now.ToString("fffffff")}@mail.ru",
                    medicalCardNumber = i,
                    getMedicalCardDate = birthDate.AddYears(3),
                    lastVisitDate = DateTime.Now.AddDays(-random.Next(0, 11)).AddMinutes(-random.Next(1, 200)),
                    nextVisitDate = DateTime.Now.AddDays(random.Next(1, 10)).AddMinutes(random.Next(1, 200)),
                    insurancePolicyNumber = $"{random.Next(0, 10000).ToString("G4")}{random.Next(0, 10000).ToString("G4")}" +
                    $"{random.Next(0, 10000).ToString("G4")}{random.Next(0, 10000).ToString("G4")}",
                    insurancePolicyEndDate = DateTime.Now.AddDays(random.Next(1, 100))
                };

                if (gender == 0)
                    fImage++;
                else
                    mImage++;

                clients.Add(newClient);
                db.clients.Add(newClient);
            }
            db.SaveChanges();
            dg.ItemsSource = clients;
        }
    }
}
