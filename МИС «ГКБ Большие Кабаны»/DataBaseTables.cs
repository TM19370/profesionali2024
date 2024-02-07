using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using МИС__ГКБ_Большие_Кабаны_.Migrations;
using static МИС__ГКБ_Большие_Кабаны_.DBInteract;

namespace МИС__ГКБ_Большие_Кабаны_
{
    public class Hospitalization
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int hospitalization_id { get; set; }
        [Required] public string firstName { get; set; }
        [Required] public string secondName { get; set; }
        [Required] public string lastName { get; set; }
        [Required] public string passportNumberAndSeries { get; set; }
        public string passportGetInfo { get; set; }
        [Required] public DateTime birthDate { get; set; }
        [Required] public virtual Gender gender { get; set; }
        /*{ 
            get 
            { 
                return gender; 
            } 
            set
            { 
                gender = db.genders.Find(value); 
            } 
        }*/
        [Required] public string workPlace { get; set; }
        [Required] public string address { get; set; }
        [Required] public string phoneNumder { get; set; }
        [Required] public string email { get; set; }
        [Required] public int medicalCardNumber { get; set; }
        [Required] public DateTime getMedicalCardDate { get; set; }
        [Required] public DateTime lastVisitDate { get; set; }
        [Required] public DateTime nextVisitDate { get; set; }
        [Required] public string insurancePolicyNumber { get; set; }
        [Required] public DateTime insurancePolicyEndDate { get; set; }
        public string insuranceCompany { get; set; }
        public string diagnosis { get; set; }
        public string hospitalizationPurpose { get; set; }
        public string hospitalizationDepartment { get; set; }
        public string hospitalizationCondition { get; set; }
        public DateTime hospitalizationStartDate { get; set; }
        public DateTime hospitalizationEndDate { get; set; }
        public string hospitalizationAddInfo { get; set; }
        public string hospitalizationCancelInfo { get; set; }
    }

    public class Account
    {
        [Key] public string login { get; set; }
        [Required] public string password { get; set; }
    }

    public class Client
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int client_id { get; set; }
        [Required] public string photoPath { get; set; }
        [Required] public string firstName { get; set; }
        [Required] public string secondName { get; set; }
        [Required] public string lastName { get; set; }
        [Required] public string passportNumberAndSeries { get; set; }
        public string passportGetInfo { get; set; }
        [Required] public DateTime birthDate { get; set; }
        [Required] public virtual Gender gender { get; set; }
        [Required] public string address { get; set; }
        [Required] public string phoneNumder { get; set; }
        [Required] public string email { get; set; }
        [Required] public int medicalCardNumber { get; set; }
        [Required] public DateTime getMedicalCardDate { get; set; }
        [Required] public DateTime lastVisitDate { get; set; }
        [Required] public DateTime nextVisitDate { get; set; }
        [Required] public string insurancePolicyNumber { get; set; }
        [Required] public DateTime insurancePolicyEndDate { get; set; }
        //[Required] public virtual Diagnosis diagnosis { get; set; } выбирать последний элемент медикал хистори с каким нибудь условием
    }

    public class Bed
    {
        [Key] public int bed_id { get; set; }
        [Required] public int roomNumber { get; set; }
        [Required] public string bedCode { get; set; }
        public virtual Client Client { get; set; }

        public void EditClient(Client client)
        {
            this.Client = client;
        }
    }

    public class MedicalHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int medicalHistory_id { get; set; }
        [Required] public virtual Client client { get; set; }
        [Required] public virtual Diagnosis diagnosis { get; set; }
        // можно добавить дату
    }

    public class Gender
    {
        [Key] public string genderName { get; set; }

        public static implicit operator Gender(string v)
        {
            Gender gender = db.genders.Where(w => w.genderName == v).FirstOrDefault();
            return gender;
        }
        /*
        public static explicit operator Gender(string v)
        {
            Gender gender = db.genders.Find(v);
            return gender;
        }*/

        public static Gender getGender(string v)
        {
            Gender gender = db.genders.Where(w => w.genderName == v).FirstOrDefault();
            return gender;
        }
    }

    public class Diagnosis
    {
        [Key] public string diagnosisName { get; set; }
    }

    public class Measure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int measure_id { get; set; }
        [Required] public virtual Client client { get; set; }
        [Required] public DateTime measureDate { get; set; }
        [Required] public virtual Doctor doctor { get; set; }
        [Required] public virtual MeasureType measureType { get; set; }
        [Required] public string measureName { get; set; }
        [Required] public string measureResault { get; set; }
        [Required] public string recommendations { get; set; }
    }

    public class Doctor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int doctor_id { get; set; }
        [Required] public string firstName { get; set; }
        [Required] public string secondName { get; set; }
        [Required] public string lastName { get; set; }
    }

    public class MeasureType
    {
        [Key] public string measureTypeName { get; set; }
    }

    public class Medicament
    {
        [Key] public string medicamentName { get; set; }
    }

    public class AppointmentInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int appointmentInfo_id { get; set; }
        [Required] public virtual Client client { get; set; }
        [Required] public string anamnesis { get; set; }
        [Required] public string symptoms { get; set; }
        [Required] public string diagnosis { get; set; }
        [Required] public string recommendations { get; set; }
    }

    public class Prescription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int prescription_id { get; set;}
        [Required] public virtual Medicament medicament { get; set; }
        [Required] public double dose { get; set; }
        [Required] public string format { get; set; }
        [Required] public virtual AppointmentInfo appointmentInfo { get; set; }
    }

    public class Timetable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int timetable_id { get; set; }
        [Required] public virtual Doctor doctor { get; set; }
        [Required] public string cabinet { get; set; }
        [Required] public DayOfWeek dayOfWeek { get; set; }
        [Required] public TimeSpan timeSpan { get; set; }
    }
}
