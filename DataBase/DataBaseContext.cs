using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base(@"Server=192.168.147.50\training;Database=MISGKBBolshieKabani;User=intern;Password=0000;Trusted_Connection=False")
        { }

        public DbSet<Hospitalization> hospitalizations { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseForMedicament> WarehouseForMedicaments { get; set; }
        public DbSet<AppointmentInfo> appointmentsInfo { get; set; }
        public DbSet<Prescription> prescriptions { get; set; }
        public DbSet<Timetable> timetables { get; set; }
        public DbSet<WeekTimetable> weekTimetable { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Account> accounts { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<MedicalHistory> medicalHistories { get; set; }
        public DbSet<Gender> genders { get; set; }
        public DbSet<Diagnosis> diagnoses { get; set; }
        public DbSet<Measure> measures { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<MeasureType> measureTypes { get; set; }
    }
}
