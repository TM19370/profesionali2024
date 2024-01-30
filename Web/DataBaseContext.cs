using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using МИС__ГКБ_Большие_Кабаны_;

namespace Web
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base(@"Server=192.168.147.50\training;Database=MISGKBBolshieKabani;User=intern;Password=0000;Trusted_Connection=False")
        { }

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
