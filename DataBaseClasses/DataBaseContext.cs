using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseClasses
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base(@"Server=192.168.147.50\training;Database=MISGKBBolshieKabani;User=intern;Password=0000;Trusted_Connection=False")
        { }

        public DbSet<Hospitalization> hospitalizations { get; set; }
        public DbSet<FullName> fullNames { get; set; }
        public DbSet<Account> accounts { get; set; }
        public DbSet<AccountType> accountTypes { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Bed> beds { get; set; }
        public DbSet<MedicalHistory> medicalHistories { get; set; }
        public DbSet<Gender> genders { get; set; }
        public DbSet<Diagnosis> diagnoses { get; set; }
        public DbSet<Measure> measures { get; set; }
        public DbSet<MeasureType> measureTypes { get; set; }
        public DbSet<Medicament> medicaments { get; set; }
        public DbSet<WarehouseForMedicament> warehouseForMedicaments { get; set; }
        public DbSet<Warehouse> warehouses { get; set; }
        public DbSet<AppointmentInfo> appointmentsInfo { get; set; }
        public DbSet<Prescription> prescriptions { get; set; }

        
        public DbSet<ScheduleElement> scheduleElements { get; set; }
        public DbSet<DayOfWeek> dayOfWeeks { get; set; }
        public DbSet<WorkTime> workTimes { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<Event> events { get; set; }
    }
}
