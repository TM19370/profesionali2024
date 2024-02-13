using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseClasses
{
    public class Hospitalization
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int hospitalization_id { get; set; }
        [Required] public virtual Client client { get; set; }
        public string? diagnosis { get; set; }
        public string? hospitalizationPurpose { get; set; }
        public string? hospitalizationDepartment { get; set; }
        public string? hospitalizationCondition { get; set; }
        public DateTime? hospitalizationStartDate { get; set; }
        public DateTime? hospitalizationEndDate { get; set; }
        public string? hospitalizationAddInfo { get; set; }
        public string? hospitalizationCancelInfo { get; set; }
        public void Edit(Hospitalization hospitalization)
        {
            this.client = hospitalization.client;
            this.diagnosis = hospitalization.diagnosis;
            this.hospitalizationPurpose = hospitalization.hospitalizationPurpose;
            this.hospitalizationDepartment = hospitalization.hospitalizationDepartment;
            this.hospitalizationCondition = hospitalization.hospitalizationCondition;
            this.hospitalizationStartDate = hospitalization.hospitalizationStartDate;
            this.hospitalizationEndDate = hospitalization.hospitalizationEndDate;
            this.hospitalizationAddInfo = hospitalization.hospitalizationAddInfo;
            this.hospitalizationCancelInfo = hospitalization.hospitalizationCancelInfo;
        }
    }

    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int account_id { get; set; }
        [Required] public string login { get; set; }
        [Required] public string password { get; set; }
    }

    public class Client
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int client_id { get; set; }
        public string? photoPath { get; set; }
        [Required] public string firstName { get; set; }
        [Required] public string secondName { get; set; }
        [Required] public string lastName { get; set; }
        [Required] public string passportNumberAndSeries { get; set; }
        [Required] public string passportGetInfo { get; set; }
        [Required] public DateTime birthDate { get; set; }
        [Required] public virtual Gender gender { get; set; }
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
        [Required] public string insuranceCompany { get; set; }
        //[Required] public virtual Diagnosis diagnosis { get; set; } выбирать последний элемент медикал хистори с каким нибудь условием

        public string fullName { get { return $"{secondName} {firstName} {lastName}"; } }
    }

    public class Bed
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int bed_id { get; set; }
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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int gender_id { get; private set; }
        [Required] public string genderName { get; set; }
    }

    public class Diagnosis
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int diagnosis_id { get; set; }
        [Required] public string diagnosisName { get; set; }
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

        public string GetFullName()
        {
            return $"{secondName} {firstName} {lastName}";
        }
    }

    public class MeasureType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int measureType_id { get; set; }
        [Required] public string measureTypeName { get; set; }
    }

    public class Medicament
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int medicament_id { get; set; }
        [Required] public string medicamentName { get; set; }
    }

    public class WarehouseForMedicament
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int warehouseForMedicament_id { get; set; }
        [Required] public virtual Warehouse warehouse { get; set; }
        [Required] public virtual Medicament medicament { get; set; }
        [Required] public int medicamentCount { get; set; }
    }

    public class Warehouse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int warehouse_id { get; set; }
        [Required] public string warehouseName { get; set; }
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
        [Required] public virtual WeekTimetable weekTimetable { get; set; }
        [Required] public DayOfWeek dayOfWeek { get; set; }
        public string? cabinet { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }

        public string GetAsString()
        {
            if (startTime == null || endTime == null)
                return "";
            return $"{((DateTime)startTime).TimeOfDay} - {((DateTime)endTime).TimeOfDay} {cabinet} каб.";
        }
    }

    public class WeekTimetable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int weekTimeTable_id { get; set; }
        [Required] public virtual Doctor doctor { get; set; }
        [Required] public DateTime weekFirstDayDate { get; set; }
    }
}
