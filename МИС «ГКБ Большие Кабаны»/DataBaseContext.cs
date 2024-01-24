using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МИС__ГКБ_Большие_Кабаны_
{
    internal class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base("DbConnection")
        { }

        public DbSet<Client> clients { get; set; }
        public DbSet<MedicalHistory> medicalHistories { get; set; }
        public DbSet<Gender> genders { get; set; }
        public DbSet<Diagnosis> diagnoses { get; set; }
        public DbSet<Measure> measures { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<MeasureType> measureTypes { get; set; }
    }
}
