using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using МИС__ГКБ_Большие_Кабаны_.Migrations;

namespace МИС__ГКБ_Большие_Кабаны_
{
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
}
