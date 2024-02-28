using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp
{
    public class FullName
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int fullName_id { get; set; }
        [Required] public string firstName { get; set; }
        [Required] public string secondName { get; set; }
        [Required] public string lastName { get; set; }

        public string GetFullName { get { return $"{secondName} {firstName} {lastName}"; } }
    }

    public class Client
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int client_id { get; set; }
        public string photoPath { get; set; }
        [Required] public virtual FullName fullName { get; set; }
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
    }

    public class Gender
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int gender_id { get; private set; }
        [Required] public string genderName { get; set; }
    }

    public class Prescription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int prescription_id { get; set; }
        [Required] public virtual Medicament medicament { get; set; }
        [Required] public double dose { get; set; }
        [Required] public string format { get; set; }
        [Required] public virtual AppointmentInfo appointmentInfo { get; set; }
    }

    public class Medicament
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int medicament_id { get; set; }
        [Required] public string medicamentName { get; set; }
    }

    public class AppointmentInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int appointmentInfo_id { get; set; }
        [Required] public virtual Client client { get; set; }
        [Required] public string anamnesis { get; set; }
        [Required] public string symptoms { get; set; }
        [Required] public string diagnosis { get; set; }
        [Required] public string recommendations { get; set; }
        public string audioMessageFileName { get; set; }
    }

}
